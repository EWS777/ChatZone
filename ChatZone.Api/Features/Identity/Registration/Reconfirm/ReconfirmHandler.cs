using ChatZone.Context;
using ChatZone.Core.Extensions;
using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Notifications;
using ChatZone.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.Features.Identity.Registration.Reconfirm;

public class ReconfirmHandler(
    ChatZoneDbContext dbContext,
    IToken token) : IRequestHandler<ReconfirmRequest, Result<IActionResult>>
{
    public async Task<Result<IActionResult>> Handle(ReconfirmRequest request, CancellationToken cancellationToken)
    {
        var person = await dbContext.Persons.SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if(person is null) return Result<IActionResult>.Failure(new NotFoundException("User is not found!"));

        person.EmailConfirmToken = token.GenerateAuthorizationToken();
        person.EmailConfirmTokenExp = DateTimeOffset.UtcNow.AddMinutes(15);

        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        await EmailSender.SendCodeToEmail(person.Email, person.EmailConfirmToken, cancellationToken);
        return Result<IActionResult>.Ok(new OkObjectResult("Link was sent!"));
    }
}