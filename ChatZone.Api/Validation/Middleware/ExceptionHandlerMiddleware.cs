using ChatZone.Core.Extensions.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Validation.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch(Exception exception)
        {
            logger.LogError(exception, "Exception occured: {Message}", exception.Message);

            var statusCode = StatusCodes.Status500InternalServerError;
            var errorMessage = "Unknown error";

            if (exception is DomainException customException)
            {
                statusCode = customException.StatusCode;
                errorMessage = customException.Message;
            }
            

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = errorMessage,
            };
            context.Response.StatusCode = statusCode;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}