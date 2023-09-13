using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models;

namespace TravelPlatform.Handler.Response
{
    public class ResponseHandler : IResponseHandler
    {
        public IActionResult ReturnResponse(ResponseDto response)
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
    }
}
