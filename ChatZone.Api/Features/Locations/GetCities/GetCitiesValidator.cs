using FluentValidation;

namespace ChatZone.Features.Locations.GetCities;

public class GetCitiesValidator : AbstractValidator<GetCitiesRequest>
{
    public GetCitiesValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100).WithMessage("Search max length is 100 characters");

        RuleFor(x => x.Take)
            .InclusiveBetween(1, 50).WithMessage("Take must be between 1 and 50");
    }
}
