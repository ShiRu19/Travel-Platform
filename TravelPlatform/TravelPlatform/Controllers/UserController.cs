using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.User;
using TravelPlatform.Services;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public UserController(TravelContext db, IConfiguration configuration, ITokenService tokenService)
        {
            _db = db;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetUserList")]
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
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel user)
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

                return SignInFB(user);
            }

            return BadRequest();
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

            var token = _tokenService.GenerateJwtToken(expectedUser);

            return Ok(new
            {
                accessToken = token.Result,
                user = new
                {
                    id = expectedUser.Id,
                    provider = expectedUser.Provider,
                    name = expectedUser.Name,
                    email = expectedUser.Email,
                }
            });
        }

        private IActionResult SignInFB(SignInModel user)
        {
            return Ok();
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
