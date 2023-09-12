using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using TravelPlatform.Models;

namespace TravelPlatform.Services.Response
{
    public interface IResponseService
    {
        IActionResult ReturnResponse(ResponseDto response);
    }
}
