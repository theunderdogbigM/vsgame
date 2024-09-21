using FluentValidation;
using gamestore.DTOs;

namespace gamestore.Validations;

public class UpdateUserValidator: AbstractValidator<UpdateUserDto>
{
public UpdateUserValidator()
    {
        RuleFor(item => item.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(item => item.Email).NotEmpty().WithMessage("Email is Required");
        RuleFor(item => item.Password).NotEmpty().WithMessage("Password is required.");
       
       
    }
}