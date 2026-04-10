namespace Core.Models;

public readonly struct Result<T, E>
{
    public bool IsOk { get; }
    private readonly T? _value;
    private readonly E? _error;

    private Result(bool isOk, T? value, E? error)
    {
        IsOk = isOk;
        _value = value;
        _error = error;
    }

    public static Result<T, E> Ok(T value) => new(true, value, default);
    public static Result<T, E> Err(E error) => new(false, default, error);

    public TOut Match<TOut>(Func<T, TOut> onOk, Func<E, TOut> onErr)
    {
        return IsOk ? onOk(_value!) : onErr(_error!);
    }
}
