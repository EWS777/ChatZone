using ChatZone.Core.Extensions.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Shared.Context;

public static class LocationValidationExtensions
{
    public static async Task<DomainException?> ValidateLocationAsync(this ChatZoneDbContext dbContext, int? idCountry, int? idCity, int? idLanguage, CancellationToken cancellationToken)
    {
        if (idCountry.HasValue && !await dbContext.Countries.AnyAsync(x => x.IdCountry == idCountry, cancellationToken)) return new BadRequestException("Country is not found!");

        if (idCity.HasValue)
        {
            if (!idCountry.HasValue) return new BadRequestException("Choose a country before choosing a city!");

            var isCityInCountry = await dbContext.Cities.AnyAsync(x => x.IdCity == idCity && x.IdCountry == idCountry, cancellationToken);
            if (!isCityInCountry) return new BadRequestException("City is not found in the chosen country!");
        }

        if (idLanguage.HasValue && !await dbContext.Languages.AnyAsync(x => x.IdLanguage == idLanguage, cancellationToken)) return new BadRequestException("Language is not found!");

        return null;
    }
}
