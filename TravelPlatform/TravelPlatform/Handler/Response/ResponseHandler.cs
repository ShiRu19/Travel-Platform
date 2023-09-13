using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models;

namespace TravelPlatform.Handler.Response
{
    public class ResponseHandler : IResponseHandler
    {
        public IActionResult ReturnResponse(ResponseDto response)
        {
            if(response.StatusCode == 200)
            {
                return new ObjectResult(new
                {
                    message = response.Message,
                    data = response.Data
                })
                {
                    StatusCode = 200
                };
            }
            else
            {
                return new ObjectResult(new
                {
                    message = response.Message,
                    error = response.Error
                })
                {
                    StatusCode = response.StatusCode
                };
            }
        }
    }
}
