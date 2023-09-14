using Amazon;
using System.Security.Claims;
using TravelPlatform.Models;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.User;
using TravelPlatform.Services.Facebook;
using TravelPlatform.Services.PasswordService;
using TravelPlatform.Services.Token;

namespace TravelPlatform.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenService _tokenService;
        private readonly IFacebookService _facebookService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordService _passwordService;

        public UserService(TravelContext db, IConfiguration configuration, IJwtTokenService tokenService, IFacebookService facebookService, IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
        {
            _db = db;
            _configuration = configuration;
            _tokenService = tokenService;
            _facebookService = facebookService;
            _httpContextAccessor = httpContextAccessor;
            _passwordService = passwordService;
        }

        /// <summary>
        /// 建立新的 facebook 註冊使用者
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CreateNewFBUserAsync(FBProfile profile)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            User newUser = new User();
            try
            {
                newUser.Id = _db.Users.Max(u => u.Id) == 0 ? 1 : _db.Users.Max(u => u.Id) + 1;
                newUser.Role = "User";
                newUser.Provider = "facebook";
                newUser.Name = profile.Name;
                newUser.Email = profile.Email;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            var token = _tokenService.GenerateJwtToken(newUser);

            newUser.AccessToken = token.Result;

            _db.Users.Add(newUser);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            response200.Data = new
            {
                accessToken = token.Result,
                user = new
                {
                    id = newUser.Id,
                    provider = newUser.Provider,
                    name = newUser.Name,
                    email = newUser.Email,
                }
            };
            return response200;
        }

        /// <summary>
        /// 取得使用者列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetUserListAsync()
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var user = _db.Users.Select(u => new
                {
                    id = u.Id,
                    name = u.Name,
                    email = u.Email,
                    provider = u.Provider
                }).ToList();

                response200.Data = user;

                return response200;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }
        }

        /// <summary>
        /// 解析 token 取得個人資料
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> ProfileAsync()
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };

            var userIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var id = userIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var provider = userIdentity.FindFirst("provider")?.Value;
            var name = userIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var email = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            var userProfile = new UserProfile
            {
                Id = long.Parse(id),
                Provider = provider,
                Name = name,
                Email = email
            };

            response200.Data = userProfile;
            return response200;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResponseDto> SignInAsync(SignInModel user)
        {
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            if (user.Provider.ToLower() == "native")
            {
                return await SignInNativeAsync(user);
            }
            else if (user.Provider.ToLower() == "facebook")
            {
                if (user.Access_token_fb == null)
                {
                    response400.StatusCode = 400;
                    response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                    response400.Message = "Facebook access token is null.";
                    return response400;
                }

                return await SignInFBAsync(user);
            }

            response400.StatusCode = 400;
            response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
            response400.Message = "Unexpected login method.";
            return response400;
        }

        /// <summary>
        /// 登入 - FB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<ResponseDto> SignInFBAsync(SignInModel user)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            FBProfile profile = await _facebookService.GetProfileAsync(user.Access_token_fb);

            if (profile.Id == null)
            {
                response400.StatusCode = 403;
                response400.Error = _configuration["ErrorMessage:FORBIDDEN"];
                response400.Message = "You don't have permission to access";
                return response400;
            }

            User expectedUser = new User();
            try
            {
                var expectedUser_query = _db.Users.SingleOrDefault(u => u.Email == profile.Email);
                if (expectedUser_query == null)
                {
                    return await CreateNewFBUserAsync(profile);
                }
                expectedUser = expectedUser_query;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            var newToken = _tokenService.GenerateJwtToken(expectedUser);

            expectedUser.AccessToken = newToken.Result;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            response200.Data = new
            {
                accessToken = newToken.Result,
                user = new
                {
                    id = expectedUser.Id,
                    provider = expectedUser.Provider,
                    name = expectedUser.Name,
                    email = expectedUser.Email,
                }
            };

            return response200;
        }

        /// <summary>
        /// 登入 - 一般
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<ResponseDto> SignInNativeAsync(SignInModel user)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            if (user.Email == null || user.Password == null)
            {
                response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                response400.Message = "Empty email or password.";
                return response400;
            }

            User expectedUser = new User();
            try
            {
                var expectedUser_query = _db.Users.SingleOrDefault(u => u.Email == user.Email);
                if (expectedUser_query == null)
                {
                    response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                    response400.Message = "User not found.";
                    return response400;
                }
                expectedUser = expectedUser_query;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            if (await _passwordService.VerifyPassword(user.Password, expectedUser.Password) == false)
            {
                response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                response400.Message = "Invalid email or password.";
                return response400;
            }

            var newToken = _tokenService.GenerateJwtToken(expectedUser);

            expectedUser.AccessToken = newToken.Result;

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            response200.Data = new
            {
                accessToken = newToken.Result,
                user = new
                {
                    id = expectedUser.Id,
                    provider = expectedUser.Provider,
                    name = expectedUser.Name,
                    email = expectedUser.Email,
                }
            };

            return response200;
        }

        /// <summary>
        /// 註冊 - 一般
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResponseDto> SignUpAsync(SignUpModel user)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            var expectedUser = _db.Users.SingleOrDefault(u => u.Email == user.Email);

            if (expectedUser != null)
            {
                response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                response400.Message = "User already exists.";
                return response400;
            }

            User newUser = new User();
            try
            {
                newUser.Id = _db.Users.Max(u => u.Id) == 0 ? 1 : _db.Users.Max(u => u.Id) + 1;
                newUser.Role = "User";
                newUser.Provider = "native";
                newUser.Name = user.Name;
                newUser.Email = user.Email;
                newUser.Password = await _passwordService.HashPassword(user.Password);
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            var token = _tokenService.GenerateJwtToken(newUser);

            newUser.AccessToken = token.Result;

            try
            {
                _db.Users.Add(newUser);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            response200.Data = new
            {
                accessToken = token.Result,
                user = new
                {
                    id = newUser.Id,
                    provider = newUser.Provider,
                    name = newUser.Name,
                    email = newUser.Email,
                }
            };

            return response200;
        }
    }
}
