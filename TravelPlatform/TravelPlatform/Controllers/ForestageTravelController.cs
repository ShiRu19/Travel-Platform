﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Handler.Response;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services;
using TravelPlatform.Services.Travel.Forestage;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ForestageTravelController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly IForestageTravelService _travelService;
        private readonly IResponseHandler _responseHandler;

        public ForestageTravelController(TravelContext db, IForestageTravelService travelService, IResponseHandler responseHandler)
        {
            _db = db;
            _travelService = travelService;
            _responseHandler = responseHandler;
        }

        /// <summary>
        /// 取得目前開放中的行程
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetTravelList")]
        public async Task<IActionResult> GetTravelList()
        {
            var response = await _travelService.GetOpenTravelList();
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// 取得行程詳細資訊
        /// </summary>
        /// <param name="id">行程 id</param>
        /// <returns>Travel detail</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetTravelDetail")]
        public async Task<IActionResult> GetTravelDetail(long id)
        {
            var response = await _travelService.GetTravelDetail(id);
            return _responseHandler.ReturnResponse(response);   
        }

        /// <summary>
        /// 取得場次詳細資訊
        /// </summary>
        /// <param name="productNumber">場次編號</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetSessionDetail")]
        public async Task<IActionResult> GetSessionDetail(string productNumber)
        {
            var response = await _travelService.GetSessionDetail(productNumber);
            return _responseHandler.ReturnResponse(response);
        }
    }
}
