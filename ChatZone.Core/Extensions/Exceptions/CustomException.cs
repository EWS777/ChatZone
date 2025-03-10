namespace ChatZone.Core.Extensions.Exceptions;

public class CustomException : Exception
{
    public int StatusCode { get; }

    private protected CustomException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public class NotFoundException(string message) : CustomException(message, 404);
public class ExistPersonException(string message) : CustomException(message, 400);
public class ForbiddenAccessException(string message) : CustomException(message, 403);