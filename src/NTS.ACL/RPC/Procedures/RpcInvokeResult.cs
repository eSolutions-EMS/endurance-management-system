namespace Core.Application.Rpc.Procedures;
public class RpcInvokeResult
{
    protected RpcInvokeResult() {}

    public bool IsSuccessful { get; protected set; }

    public static RpcInvokeResult Success
        => new() { IsSuccessful = true };

    public static RpcInvokeResult Error
        => new();
}

public class RpcInvokeResult<T> : RpcInvokeResult
{
    private RpcInvokeResult() { }

    public T? Data { get; private set; }

    public static new RpcInvokeResult<T> Success(T data)
        => new() { Data = data, IsSuccessful = true };

    public static new RpcInvokeResult<T> Error
        => new() {};
}
