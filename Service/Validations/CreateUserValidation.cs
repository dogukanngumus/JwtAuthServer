using Core.Dtos;
using FluentValidation;

namespace Service.Validations;

public class CreateUserValidation : AbstractValidator<CreateUserDto>
{
    public CreateUserValidation()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotNull().NotEmpty();
        RuleFor(x => x.UserName).NotNull().NotEmpty();

    }
}