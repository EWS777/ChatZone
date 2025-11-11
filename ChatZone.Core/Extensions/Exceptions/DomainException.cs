namespace ChatZone.Core.Extensions.Exceptions;

public class DomainException : Exception
{
    public int StatusCode { get; }

    private protected DomainException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public class NotFoundException(string message) : DomainException(message, 404);
public class ExistPersonException(string message) : DomainException(message, 409);
public class ForbiddenAccessException(string message) : DomainException(message, 403);
public class NotAnyDataException(string message) : DomainException(message, 204);
public class IsExistsException(string message) : DomainException(message, 409); //409 - Conflict when this data is exists
public class ExpiredTokenException(string message) : DomainException(message, 410); //410 -> means that it was but not is not available
public class BackendException() : DomainException("Something went wrong. Please try again", 500);