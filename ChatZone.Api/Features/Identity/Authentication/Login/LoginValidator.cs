using System.Text.RegularExpressions;
using FluentValidation;

namespace ChatZone.Features.Identity.Authentication.Login;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .Custom((usernameOrEmail, context) =>
            {
                if (usernameOrEmail.Contains('@'))
                {
                    if(!Regex.IsMatch(usernameOrEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) context.AddFailure("Email format is incorrect");
                    else if(usernameOrEmail.Trim().Replace(" ", "").Length > 51) context.AddFailure("Email max length is 50 characters");
                }
                else
                {
                    if(usernameOrEmail.Trim().Replace(" ", "").Length < 5) context.AddFailure("Username min length is 6 characters");
                    if(usernameOrEmail.Trim().Replace(" ", "").Length > 21) context.AddFailure("Username max length is 20 characters");
                }
            });
        
        RuleFor(x => x.Password)
            .Must(x => x.Trim().Replace(" ", "").Length > 7).WithMessage("Password min length is 8 characters");
    }
}