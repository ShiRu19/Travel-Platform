using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using TravelPlatform.Handler.Response;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Record;
using TravelPlatform.Services.DashboardService;

namespace TravelPlatform.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]

    public class DashboardController : ControllerBase
    {
        private readonly TravelContext _db;
        private IConfiguration _configuration;
        private IDashboardService _dashboardService;
        private IResponseHandler _responseHandler;
        public DashboardController(TravelContext db, IConfiguration configuration, IDashboardService dashboardService, IResponseHandler responseHandler)
        {
            _db = db;
            _configuration = configuration;
            _dashboardService = dashboardService;
            _responseHandler = responseHandler;
        }

        /// <summary>
        /// Get sales volume over a date range
        /// </summary>
        /// <param name="nation">To inquire about the nation</param>
        /// <param name="start">Start of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>Sales volume</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetSalesVolume")]
        public async Task<IActionResult> GetSalesVolume(string nation, DateTime start, DateTime end)
        {
            var response = await _dashboardService.GetSalesVolumeAsync(nation, start, end);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// Get sales over a date range
        /// </summary>
        /// <param name="nation">To inquire about the nation</param>
        /// <param name="start">Start of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>Sales</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetSales")]
        public async Task<IActionResult> GetSales(string nation, DateTime start, DateTime end)
        {
            var response = await _dashboardService.GetSalesAsync(nation, start, end);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// Get domestic sales volume over a date range
        /// </summary>
        /// <param name="start">Start of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>Domestic sales volume</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetDomesticSalesVolume")]
        public async Task<IActionResult> GetDomesticSalesVolume(DateTime start, DateTime end)
        {
            var response = await _dashboardService.GetDomesticSalesVolumeAsync(start, end);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// Get domestic group status over a date range
        /// </summary>
        /// <param name="start">Start of date range</param>
        /// <param name="end">End of date range</param>
        /// <returns>Domestic group status</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetDomesticGroupStatus")]
        public async Task<IActionResult> GetDomesticGroupStatus(DateTime start, DateTime end)
        {
            var response = await _dashboardService.GetDomesticGroupStatusAsync(start, end);
            return _responseHandler.ReturnResponse(response);
        }

        /// <summary>
        /// Get top five follow record of open travel
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetFollowTopFive")]
        public async Task<IActionResult> GetFollowTopFive()
        {
            var response = await _dashboardService.GetFollowTopFiveAsync();
            return _responseHandler.ReturnResponse(response);
        }
    }
}
