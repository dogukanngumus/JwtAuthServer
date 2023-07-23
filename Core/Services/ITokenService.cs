using Core.Configuration;
using Core.Dtos;
using Core.Entities;

namespace Core.Services;

public interface ITokenService
{
    TokenDto CreateToken(UserApp userApp);
    ClientTokenDto CreateClientToken(Client client);
}