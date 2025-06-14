using System.IdentityModel.Tokens.Jwt;
using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Notifications;
using ChatZone.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Password.Reset;

public class ResetPasswordHandler(
    ChatZoneDbContext dbContext,
    IToken token) : IRequestHandler<ResetPasswordRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.Email==request.Email, cancellationToken);
        if (person is null) return Result<IActionResult>.Failure(new NotFoundException("User is not found!"));

        var generatedToken = token.GenerateJwtToken(person.Username, person.Role, person.IdPerson);
        await EmailSender.SendCodeToEmail(person.Email, new JwtSecurityTokenHandler().WriteToken(generatedToken), cancellationToken);

        return Result<IActionResult>.Ok(new OkResult());
    }
}