using Not.Application.RPC.Clients;

namespace Not.Tests.RPC;

public interface ITestRpcClient : IRpcClient
{
    List<string> InvokedMethods { get; }
}
