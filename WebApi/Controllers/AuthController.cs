using Core.Dtos;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> CreateToken([FromBody]LoginDto loginDto)
    {
        var result = await _authenticationService.CreateToken(loginDto);
        return ActionResultInstance(result);
    }
    
    [HttpPost("clientlogin")]
    public IActionResult CreateTokenByClient([FromBody]ClientLoginDto client)
    {
        var result =  _authenticationService.CreateTokenByClient(client);
        return ActionResultInstance(result);
    }
    
    [HttpPost("loginWithRefreshToken")]
    public async Task<IActionResult> LoginWithRefreshToken([FromBody]RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.CreateTokenWithRefreshToken(refreshTokenDto.RefreshToken);
        return ActionResultInstance(result);
    }

    [HttpPost("revokeRefreshToken")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.RefreshToken);
        return ActionResultInstance(result);
    }
}