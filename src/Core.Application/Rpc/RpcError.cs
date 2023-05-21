using System;

namespace Core.Application.Rpc;

public class RpcError
{
    public RpcError(Exception exception, string procedure, params object[] arguments)
    {
        this.Exception = exception;
        this.Procedure = procedure;
        this.Arguments = arguments;
    }

    public Exception Exception { get; }
    public string Procedure { get; }
    public object[] Arguments { get; }
}
