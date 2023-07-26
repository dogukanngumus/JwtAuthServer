using System.Reflection;
using System.Text;
using Core.Configuration;
using Core.Dtos;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Repositories;
using Repository.UnitOfWork;
using Shared.Configurations;
using Service.Services;
using Service.Validations;
using Shared.Extensions;
using WebApi.Filters;

namespace WebApi;

public static class Startup
{
    public static WebApplication InitializeApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        ConfigureServices(builder);
        var app = builder.Build();
        ConfigureApplication(app);
        return app;
    }
    
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new ValidationFilter());
        }).ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped(typeof(IService<,>), typeof(ServiceGeneric<,>));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        });
        
        // identity configuration
        builder.Services.AddIdentity<UserApp,IdentityRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.AllowedUserNameCharacters = "abcçdefghiıjklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._@+";
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
        // authentication configuration
        builder.Services.AddCustomAuth(tokenOptions);
        
        builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
        builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidation>();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
    
    private static void ConfigureApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}