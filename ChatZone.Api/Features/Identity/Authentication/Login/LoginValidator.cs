using System.Text.RegularExpressions;
using FluentValidation;

namespace ChatZone.Features.Identity.Authentication.Login;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email or username is required!")
            .Custom((usernameOrEmail, context) =>
            {
                if (usernameOrEmail.Contains('@'))
                {
                    if(!Regex.IsMatch(usernameOrEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) context.AddFailure("Email format is incorrect");
                    else if(usernameOrEmail.Trim().Replace(" ", "").Length > 265) context.AddFailure("Email max length is 264 characters");
                }
                else
                {
                    if(usernameOrEmail.Trim().Replace(" ", "").Length < 7) context.AddFailure("Username min length is 8 characters");
                    if(usernameOrEmail.Trim().Replace(" ", "").Length > 31) context.AddFailure("Username max length is 30 characters");
                }
            });
        
    }
}