using Core.Dtos;
using Shared.Dtos;

namespace Core.Services;

public interface IAuthenticationService
{
    Task<Response<TokenDto>> CreateToken(LoginDto loginDto);
    Task<Response<TokenDto>> CreateTokenWithRefreshToken(string refreshToken);
    Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);
    Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDtos);
}