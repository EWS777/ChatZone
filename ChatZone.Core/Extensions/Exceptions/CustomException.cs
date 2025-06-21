﻿namespace ChatZone.Core.Extensions.Exceptions;

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
public class NotAnyDataException(string message) : CustomException(message, 204);
public class IsExistsException(string message) : CustomException(message, 409); //409 - Conflict when this data is exists
public class ExpiredTokenException(string message) : CustomException(message, 410); //410 -> means that it was but not is not available