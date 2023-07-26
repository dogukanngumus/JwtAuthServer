using Core.Dtos;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserController:BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody]CreateUserDto createUserDto)
    {
        return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        return ActionResultInstance(await _userService.GetUserByName(HttpContext.User.Identity.Name));
    }

    [Authorize]
    [HttpPost("{userName}")]
    public async Task<IActionResult> CreateUserRoles([FromRoute]string userName)
    {
        return ActionResultInstance(await _userService.CreateUserRoles(userName));
    }
}