using FluentValidation.Results;

namespace ChatZone.Core.Extensions;

public class Result<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;
    private readonly List<ValidationFailure>? _validationFailures;
    public bool IsSuccess { get; }
    
    public Exception Exception => _exception ?? throw new InvalidOperationException("Exception is not set!");
    public T Value => _value ?? throw new InvalidOperationException("Value is not set!");
    public IReadOnlyList<ValidationFailure> ValidationFailures => _validationFailures ?? new List<ValidationFailure>();
    
    private Result(bool isSuccess, Exception? exception, T? value, List<ValidationFailure>? validationFailures)
    {
        IsSuccess = isSuccess;
        _exception = exception;
        _value = value;
        _validationFailures = validationFailures;
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(true, null, value, null);
    }

    public static Result<T> Failure(Exception exception)
    {
        return new Result<T>(false, exception, default, null);
    }

    public static Result<T> Failure(List<ValidationFailure> validationFailures)
    {
        return new Result<T>(false, default, default, validationFailures);
    }

    public TResult Match<TResult>(Func<T, TResult> ifSuccessful, Func<Exception, TResult> ifFailure)
    {
        return IsSuccess ? ifSuccessful(Value) : ifFailure(Exception);
    }
}