using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlatform.Handler.Response;
using TravelPlatform.Models.BackstageTravel;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services.File.FileUpload;
using TravelPlatform.Services.TravelService.Backstage;

namespace TravelPlatform.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BackstageTravelController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly IFileUploadService _fileUploadService;
        private readonly IBackstageTravelAddService _travelAddService;
        private readonly IBackstageTravelEditService _travelEditService;
        private readonly IBackstageTravelListService _travelListService;
        private readonly IBackstageTravelInfoService _travelInfoService;
        private readonly IResponseHandler _responseHandler;

        public BackstageTravelController(TravelContext db, IFileUploadService fileUploadService,
            IBackstageTravelAddService travelAddService, IBackstageTravelEditService travelEditService, 
            IBackstageTravelListService travelListService, IBackstageTravelInfoService travelInfoService, 
            IResponseHandler responseHandler)
        {
            _db = db;
            _fileUploadService = fileUploadService;
            _travelAddService = travelAddService;
            _travelEditService = travelEditService;
            _travelListService = travelListService;
            _travelInfoService = travelInfoService;
            _responseHandler = responseHandler;
        }

        /// <summary>
        /// 取得開放中行程
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetOpenTravelList")]
        public async Task<IActionResult> GetOpenTravelList()
        {
            var response = await _travelListService.GetOpenTravelListAsync();
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得已關閉行程
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetCloseTravelList")]
        public async Task<IActionResult> GetCloseTravelList()
        {
            var response = await _travelListService.GetCloseTravelListAsync();
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得開放中場次
        /// </summary>
        /// <returns>Backstage Travel List Object</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetOpenTravelSessionList")]
        public async Task<IActionResult> GetOpenTravelSessionList(long id)
        {
            var response = await _travelListService.GetOpenTravelSessionListAsync(id);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得已關閉場次
        /// </summary>
        /// <returns>Backstage Travel List Object</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetCloseTravelSessionList")]
        public async Task<IActionResult> GetCloseTravelSessionList(long id)
        {
            var response = await _travelListService.GetCloseTravelSessionListAsync(id);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 新增行程
        /// </summary>
        /// <param name="travelAddModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("AddTravel")]
        public async Task<IActionResult> AddTravel([FromForm] TravelAddModel travelAddModel)
        {
            var response = await _travelAddService.AddTravelAsync(travelAddModel);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 新增場次
        /// </summary>
        /// <param name="sessionAddModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("AddSession")]
        public async Task<IActionResult> AddSession([FromForm] SessionAddModel sessionAddModel)
        {
            var response = await _travelAddService.AddSessionAsync(sessionAddModel);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得行程資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetTravelInfo")]
        public async Task<IActionResult> GetTravelInfo(long id)
        {
            var response = await _travelInfoService.GetTravelInfoAsync(id);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得場次資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetSessionInfo")]
        public async Task<IActionResult> GetSessionInfo(long id)
        {
            var response = await _travelInfoService.GetSessionInfoAsync(id);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 編輯行程
        /// </summary>
        /// <param name="travelEditModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("EditTravel")]
        public async Task<IActionResult> EditTravel([FromForm] TravelEditModel travelEditModel)
        {
            var response = await _travelEditService.EditTravelAsync(travelEditModel);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 編輯場次
        /// </summary>
        /// <param name="sessionEditModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("EditSession")]
        public async Task<IActionResult> EditSession([FromForm] SessionEditModel sessionEditModel)
        {
            var response = await _travelEditService.EditSessionAsync(sessionEditModel);
            return _responseHandler.ReturnResponse(response);
        }
    }
}
