using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Configuration;
using Core.Dtos;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Service.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<UserApp> _userManager;
    private readonly CustomTokenOptions _customTokenOptions;
    private readonly DateTime _accessTokenExpiration;
    private readonly DateTime _refreshTokenExpiration;

    public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions> customTokenOptions)
    {
        _userManager = userManager;
        _customTokenOptions = customTokenOptions.Value;
        _accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);
        _refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);
    }

    public TokenDto CreateToken(UserApp userApp)
    {
        var signin = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_customTokenOptions.SecurityKey));
        var signinCredentials = new SigningCredentials(signin, SecurityAlgorithms.HmacSha256Signature);
        var claims = CreateClaims(userApp, _customTokenOptions.Audiences).Result;
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer:_customTokenOptions.Issuer,
            expires: _accessTokenExpiration,
            claims: claims,
            signingCredentials:signinCredentials
            );

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);
        return new TokenDto()
        {
            AccessToken = token,
            AccessTokenExpiration = _accessTokenExpiration,
            RefreshToken = CreateRefreshToken(),
            RefreshTokenExpiration = _refreshTokenExpiration
        };
    }

    public ClientTokenDto CreateClientToken(Client client)
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_customTokenOptions.SecurityKey));

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOptions.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: CreateClientClaims(client),
            signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.WriteToken(jwtSecurityToken);

        var tokenDto = new ClientTokenDto
        {
            AccessToken = token,

            AccessTokenExpiration = accessTokenExpiration,
        };
        return tokenDto;
    }

    private List<Claim> CreateClientClaims(Client client)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub,client.Id.ToString())
        };
        
        claims.AddRange(client.Audiences.Select(x=> new Claim(JwtRegisteredClaimNames.Aud,x)));
        return claims;
    }
    private async Task<List<Claim>> CreateClaims(UserApp userApp, List<string> audiences)
    {
        var userRoles = await _userManager.GetRolesAsync(userApp);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, userApp.Id),
            new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
            new Claim(ClaimTypes.Name, userApp.UserName),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim("customClaim", "secretClaim"),
        };
        
        claims.AddRange(audiences.Select(x=> new Claim(JwtRegisteredClaimNames.Aud,x)));
        claims.AddRange(userRoles.Select(x=> new Claim(ClaimTypes.Role, x)));
        return claims;
    }
    
    private string CreateRefreshToken()
    {
        var numberByte = new Byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }
}