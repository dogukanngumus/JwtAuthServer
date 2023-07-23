using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    [NonAction]
    public IActionResult ActionResultInstance<T>(Response<T> response) where T:class
    {
        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}