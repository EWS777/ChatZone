using ChatZone.Core.Extensions.Exceptions;
using ChatZone.Core.Notifications;
using ChatZone.Shared.Context;
using ChatZone.Shared.DTOs;
using ChatZone.Shared.Security;
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

        person.EmailConfirmToken = token.GenerateAuthorizationToken();
        dbContext.Persons.Update(person);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await EmailSender.ResetPassword(person.Email, person.EmailConfirmToken, cancellationToken);
        return Result<IActionResult>.Ok(new OkObjectResult(new {message = "The reset link was sent to your email!"}));
    }
}