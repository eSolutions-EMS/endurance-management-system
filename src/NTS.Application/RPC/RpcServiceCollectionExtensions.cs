using Microsoft.Extensions.DependencyInjection;

namespace NTS.Application.RPC;

public static class RpcServiceCollectionExtensions
{
    public static IServiceCollection ConfigureRpcSocket(this IServiceCollection services, RpcProtocl protocol, string host, int port, string hubPattern)
    {
        var context = new RpcContext(protocol, host, port, hubPattern);
        var socket = new SignalRSocket(context);
        services.AddSingleton<IRpcSocket>(socket);
        return services;
    }
}
