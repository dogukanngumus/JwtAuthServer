using Microsoft.AspNetCore.Authorization;

namespace MiniApp1.Requirements;

public class BirthDayRequirement : IAuthorizationRequirement
{
    public BirthDayRequirement(int age)
    {
        Age = age;
    }

    public int Age  { get; set; }
}

public class BirthDayRequirementHandler : AuthorizationHandler<BirthDayRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context
        , BirthDayRequirement requirement)
    {
        var birthDate = context.User.FindFirst("birth-date");
        if (birthDate == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }
           
        var today = DateTime.Now;
        var age = today.Year - DateTime.Parse(birthDate.Value).Year;
        if (age >= requirement.Age)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }
}