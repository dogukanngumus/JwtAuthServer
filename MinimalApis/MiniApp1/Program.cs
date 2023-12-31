using Microsoft.AspNetCore.Authorization;
using MiniApp1.Requirements;
using Shared.Configurations;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
builder.Services.AddCustomAuth(tokenOptions);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("customPolicy", policy =>
    {
        policy.RequireClaim("customClaim","secretClaim");
    });
    
    options.AddPolicy("agePolicy", policy =>
    {
        policy.Requirements.Add(new BirthDayRequirement(18));
    });
});
builder.Services.AddScoped<IAuthorizationHandler, BirthDayRequirementHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();