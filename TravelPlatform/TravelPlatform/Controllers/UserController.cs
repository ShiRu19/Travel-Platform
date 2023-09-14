using Amazon;
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
using TravelPlatform.Handler.Response;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Record;
using TravelPlatform.Models.User;
using TravelPlatform.Services.Facebook;
using TravelPlatform.Services.Token;
using TravelPlatform.Services.UserService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUserService _userService;

        public UserController(IUserService userService,IResponseHandler responseHandler)
        {
            _responseHandler = responseHandler;
            _userService = userService;
        }

        /// <summary>
        /// 解析 token 取得個人資料
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("Profile"), Authorize]
        public async Task<IActionResult> Profile()
        {
            var response = await _userService.ProfileAsync();
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 確認管理者身分
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("CheckAdminRole")]
        [Authorize(Roles = "Admin")]
        public IActionResult CheckAdminRole()
        {
            return Ok();
        }

        /// <summary>
        /// 取得使用者列表
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetUserList")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserList()
        {
            var response = await _userService.GetUserListAsync();
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 註冊 - 一般
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel user)
        {
            var response = await _userService.SignUpAsync(user);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel user)
        {
            var response = await _userService.SignInAsync(user);
            return _responseHandler.ReturnResponse(response);
        }
    }
}
