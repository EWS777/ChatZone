using ChatZone.Core.Extensions.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.Shared.Middlewares;

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
            if (exception is ValidationException validationException)
            {
                logger.LogError("Validation failed: {Message}", validationException.Message);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var validationProblemDetails = new HttpValidationProblemDetails(
                    validationException.Errors
                        .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                        .ToDictionary(x => x.Key, x => x.ToArray())
                )
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed"
                };

                await context.Response.WriteAsJsonAsync(validationProblemDetails);
                return;
            }
            
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