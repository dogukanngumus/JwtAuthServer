using System.Reflection;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class AppDbContext : IdentityDbContext<UserApp, IdentityRole,string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}