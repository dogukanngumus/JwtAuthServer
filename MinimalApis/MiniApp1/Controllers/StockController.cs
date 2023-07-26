using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    [Authorize(Roles = "admin,manager",Policy = "customPolicy")]
    [HttpGet]
    public IActionResult GetStock()
    {
        var userName = HttpContext.User.Identity.Name;
        var userId = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
        return Ok($"Stock İşlemleri => UserName:{userName} UserId:{userId}");
    }
}