using NTS.ACL.Handshake;
using Not.Injection;
using NTS.Storage;

namespace NTS.Judge.MAUI.Server;

public static class HubInjection
{
    public static IServiceCollection ConfigureHub(this IServiceCollection services)
    {
        services.AddSignalR();
        services
            .AddHostedService<NetworkBroadcastService>()
            .RegisterConventionalServices()
            .ConfigureStorage();

        return services;
    }
}
