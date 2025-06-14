using FluentValidation;

namespace ChatZone.Features.QuickMessages.Create;

public class CreateQuickMessageValidator : AbstractValidator<CreateQuickMessageRequest>
{
    public CreateQuickMessageValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message can't be empty!")
            .MaximumLength(250).WithMessage("Message can't be longer than 250 characters!");
    }
}