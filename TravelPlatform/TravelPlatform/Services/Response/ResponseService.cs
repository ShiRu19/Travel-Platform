using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models;

namespace TravelPlatform.Services.Response
{
    public class ResponseService : IResponseService
    {
        public IActionResult ReturnResponse(ResponseDto response)
        {
            if (response.StatusCode == 200)
            {
                if (response.Data != null)
                {
                    return new OkObjectResult(response.Data);
                }
                return new OkResult();
            }
            else
            {
                return new ObjectResult(new
                {
                    error = response.Error,
                    message = response.Message
                })
                {
                    StatusCode = response.StatusCode
                };
            }
        }
    }
}
