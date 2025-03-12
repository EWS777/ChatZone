using ChatZone.DTO.Requests;
using FluentValidation;

namespace ChatZone.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(50).WithMessage("Email max length is 50 characters");
        
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(6).WithMessage("Username min length is 6 characters")
            .MaximumLength(20).WithMessage("Username max length is 20 characters");
        
        RuleFor(x=>x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password min length is 8 characters");
    }
}