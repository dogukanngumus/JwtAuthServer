using Core.Dtos;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Service.Mappings;
using Shared.Dtos;

namespace Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserApp> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new UserApp()
        {
            Id = Guid.NewGuid().ToString(),
            Email = createUserDto.Email,
            UserName = createUserDto.UserName
        };
        var result = await _userManager.CreateAsync(user, createUserDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            return Response<UserAppDto>.Fail(new ErrorDto(errors,true),400);
        }
        return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user),200);
    }

    public async Task<Response<UserAppDto>> GetUserByName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return Response<UserAppDto>.Fail("User not found",404,true);
        }

        return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
    }

    public async Task<Response<NoDataDto>> CreateUserRoles(string userName)
    {
        if (!await _roleManager.RoleExistsAsync("admin"))
            await _roleManager.CreateAsync(new() { Name = "admin" });
        if (!await _roleManager.RoleExistsAsync("manager"))
            await _roleManager.CreateAsync(new() { Name = "manager" });
        
        var user = await _userManager.FindByNameAsync(userName);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user,"admin");
            await _userManager.AddToRoleAsync(user,"manager");
            return Response<NoDataDto>.Success((int)StatusCodes.Status201Created);
        }
        return Response<NoDataDto>.Fail(new ErrorDto(new List<string>(){"Kullanıcı bulunamadı."},true),(int)StatusCodes.Status400BadRequest);
    }
}