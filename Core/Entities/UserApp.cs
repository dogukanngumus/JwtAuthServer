using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class UserApp : IdentityUser<string>
{
    public DateTime BirthDate { get; set; }
}