using ChatZone.DTO.Requests;
using FluentValidation;

namespace ChatZone.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email must contain a dot after '@'")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(50).WithMessage("Email max length is 50 characters");
        
        RuleFor(x => x.Username)
            .Must(x=>!x.Contains('@')).WithMessage("Username can't contains '@'")
            .Must(x=>x.Trim().Replace(" ", "").Length>5).WithMessage("Username min length is 6 characters")
            .Must(x=>x.Trim().Replace(" ", "").Length<21).WithMessage("Username max length is 20 characters");
        
        RuleFor(x => x.Password)
            .Must(x => x.Trim().Replace(" ", "").Length > 7).WithMessage("Password min length is 8 characters");
    }
}