using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.User;
using TravelPlatform.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly ITokenService _tokenService;
        private readonly IFacebookService _facebookService;

        public UserController(TravelContext db, ITokenService tokenService, IFacebookService facebookService)
        {
            _db = db;
            _tokenService = tokenService;
            _facebookService = facebookService;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("Profile"), Authorize]
        public IActionResult Profile()
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
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

            return Ok(userProfile);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("CheckAdminRole")]
        [Authorize(Roles = "Admin")]
        public IActionResult CheckAdminRole()
        {
            return Ok();
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetUserList")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserList()
        {
            try
            {
                var user = _db.Users.Select(u => new
                {
                    id = u.Id,
                    name = u.Name,
                    email = u.Email,
                    provider = u.Provider
                }).ToList();

                var result = new
                {
                    data = user
                };

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel user)
        {
            var expectedUser = _db.Users.SingleOrDefault(u => u.Email == user.Email);

            if(expectedUser != null)
            {
                return BadRequest(new
                {
                    error = "User already exists.",
                    message = "Please sign in or change email."
                });
            }

            User newUser = new User
            {
                Id = _db.Users.Max(u => u.Id) == 0 ? 1 : _db.Users.Max(u => u.Id) + 1,
                Role = user.Role,
                Provider = "native",
                Name = user.Name,
                Email = user.Email,
                Password = HashPassword(user.Password)
            };

            var token = _tokenService.GenerateJwtToken(newUser);

            newUser.AccessToken = token.Result;

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return Ok(new
            {
                accessToken = token.Result,
                user = new
                {
                    id = newUser.Id,
                    provider = newUser.Provider,
                    name = newUser.Name,
                    email = newUser.Email,
                }
            });
        }

        [MapToApiVersion("1.0")]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel user)
        {
            if(user.Provider.ToLower() == "native")
            {
                return SignInNative(user);
            }
            else if(user.Provider.ToLower() == "facebook")
            {
                if (user.Access_token_fb == null)
                {
                    return BadRequest(new
                    {
                        error = "Facebook access token is null.",
                        message = "Please try again"
                    });
                }

                return await SignInFB(user);
            }

            return BadRequest(new
            {
                error = "Unexpected login method.",
                message = "Please try again"
            });
        }

        private IActionResult SignInNative(SignInModel user)
        {
            if (user.Email == null || user.Password == null)
            {
                return BadRequest(new
                {
                    error = "Empty email or password.",
                    message = "Please try again."
                });
            }

            var expectedUser = _db.Users.SingleOrDefault(u => u.Email == user.Email);
            if (expectedUser == null)
            {
                return NotFound(new
                {
                    error = "User not found.",
                    message = "Please sign up first."
                });
            }

            if(!VerifyPassword(user.Password, expectedUser.Password))
            {
                return BadRequest(new
                {
                    error = "Invalid email or password.",
                    message = "Please try again."
                });
            }

            var newToken = _tokenService.GenerateJwtToken(expectedUser);

            expectedUser.AccessToken = newToken.Result;
            _db.SaveChanges();

            return Ok(new
            {
                accessToken = newToken.Result,
                user = new
                {
                    id = expectedUser.Id,
                    provider = expectedUser.Provider,
                    name = expectedUser.Name,
                    email = expectedUser.Email,
                }
            });
        }

        private async Task<IActionResult> SignInFB(SignInModel user)
        {
            FBProfile profile = await _facebookService.GetProfileAsync(user.Access_token_fb);
            
            if(profile.Id == null)
            {
                return Forbid();
            }

            var expectedUser = _db.Users.SingleOrDefault(u => u.Email == profile.Email);
            if (expectedUser == null)
            {
                return CreateNewUser_FB(profile);
            }

            var newToken = _tokenService.GenerateJwtToken(expectedUser);

            expectedUser.AccessToken = newToken.Result;
            _db.SaveChanges();

            return Ok(new
            {
                accessToken = newToken.Result,
                user = new
                {
                    id = expectedUser.Id,
                    provider = expectedUser.Provider,
                    name = expectedUser.Name,
                    email = expectedUser.Email,
                }
            });
        }

        private IActionResult CreateNewUser_FB(FBProfile profile)
        {
            User newUser = new User()
            {
                Id = _db.Users.Max(u => u.Id) == 0 ? 1 : _db.Users.Max(u => u.Id) + 1,
                Role = "user",
                Provider = "facebook",
                Name = profile.Name,
                Email = profile.Email
            };

            var token = _tokenService.GenerateJwtToken(newUser);

            newUser.AccessToken = token.Result;

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return Ok(new
            {
                accessToken = token.Result,
                user = new
                {
                    id = newUser.Id,
                    provider = newUser.Provider,
                    name = newUser.Name,
                    email = newUser.Email,
                }
            });
        }

        private string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            bool result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return result;
        }
    }
}
