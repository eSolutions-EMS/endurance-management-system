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
        => new Result();
    public static Result<T> Success<T>(T data)
        where T : class
        => new Result<T>(data);
    public static Result Failure(params string[] errors)
        => new Result(errors);
}

public class Result<T> : ResultBase
    where T : class
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
        this.IsSuccessful = true;
    }
    protected ResultBase(IEnumerable<string> errors)
    {
        this.Errors = errors.ToArray();
    }

    public bool IsSuccessful { get; }
    public string[] Errors { get; } = Array.Empty<string>();
}
