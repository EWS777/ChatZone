namespace ChatZone.Core.Extensions;

public class Result<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;
    public bool IsSuccess { get; }
    
    public Exception Exception => _exception ?? throw new InvalidOperationException("Exception is not set!");
    public T Value => _value ?? throw new InvalidOperationException("Value is not set!");
    
    private Result(bool isSuccess, Exception? exception, T? value)
    {
        IsSuccess = isSuccess;
        _exception = exception;
        _value = value;
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(true, null, value);
    }

    public static Result<T> Failure(Exception exception)
    {
        return new Result<T>(false, exception, default);
    }

    public TResult Match<TResult>(Func<T, TResult> ifSuccessful, Func<Exception, TResult> ifFailure)
    {
        return IsSuccess ? ifSuccessful(Value) : ifFailure(Exception);
    }
}