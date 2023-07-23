using Core.Dtos;
using Shared.Dtos;

namespace Core.Services;

public interface IUserService
{
    Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<UserAppDto>> GetUserByName(string userName);
}