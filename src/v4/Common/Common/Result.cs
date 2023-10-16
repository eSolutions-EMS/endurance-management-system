namespace Core.Models;

public class Result : ResultBase
{
    internal Result()
    {
    }
    internal Result(IEnumerable<string> errors) : base(errors)
    {
    }

    public static Result Success()
        => new();
    public static Result<T> Success<T>(T data)
        => new(data);
    public static Result Failure(params string[] errors)
        => new(errors);
}

public class Result<T> : ResultBase
{
    internal Result(T data)
    {
        this.Data = data;
    }

    public T? Data { get; }
}

public abstract class ResultBase
{
    protected ResultBase()
    {
    }
    protected ResultBase(IEnumerable<string> errors)
    {
        this.Errors = errors.ToArray();
    }

    public bool IsError => this.Errors.Any();
    public string[] Errors { get; } = Array.Empty<string>();
}
