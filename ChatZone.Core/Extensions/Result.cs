using ChatZone.Core.Extensions.Exceptions;

namespace ChatZone.Core.Extensions;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public int? StatusCode { get; set; }
    public Exception? Exception { get; set; }

    protected Result(bool isSuccess, string? errorMessage, int? statusCode, Exception? exception)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        Exception = exception;
    }

    public static Result Ok()
    {
        return new Result(true, null, null, null);
    }

    public static Result FailResult(Exception exception)
    {
        return new Result(false,exception.Message, (exception as CustomException)?.StatusCode,null);
    }
    
    public TResult Final<TResult>(TResult ifSuccess, Func<int?, string?, Exception?, TResult> ifFailure)
    {
        return IsSuccess ? ifSuccess : ifFailure(StatusCode, ErrorMessage, Exception);
    }

    
}


public class Result<T> : Result
{
    public T? Value { get; set; }

    private Result(bool isSuccess, string? errorMessage, int? statusCode, Exception? exception, T? value) : base(isSuccess, errorMessage, statusCode, exception)
    {
        Value = value;
    }
    
    public static Result<T> Ok(T value)
    {
        return new Result<T>(true, null, null, null, value);
    }

    public static Result<T> FailResultT(Exception exception)
    {
        return new Result<T>(false, exception.Message, (exception as CustomException)?.StatusCode, exception, default);
    }

    public TResult Final<TResult>(Func<T?, TResult> ifSuccess, Func<int?, string?, Exception?, TResult> ifFailure)
    {
        return IsSuccess ? ifSuccess(Value) : ifFailure(StatusCode, ErrorMessage, Exception);
    }
}