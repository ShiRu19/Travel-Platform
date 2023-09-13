using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Models;

namespace TravelPlatform.Handler.Response
{
    public interface IResponseHandler
    {
        IActionResult ReturnResponse(ResponseDto response);
    }
}
