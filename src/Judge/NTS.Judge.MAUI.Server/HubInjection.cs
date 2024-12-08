using NTS.ACL.Handshake;
using Not.Injection;
using NTS.Storage;
using Not.Serialization;
using Not.Localization;

namespace NTS.Judge.MAUI.Server;

public static class HubInjection
{
    public static IServiceCollection ConfigureHub(this IServiceCollection services)
    {
        services
            .AddSignalR(x => x.EnableDetailedErrors = true)
            .AddNewtonsoftJsonProtocol(x =>
            {
                x.PayloadSerializerSettings = SerializationExtensions.SETTINGS;
            });

        services
            .AddHostedService<NetworkBroadcastService>()
            .AddDummyLocalizer()
            .RegisterConventionalServices()
            .ConfigureStorage();

        return services;
    }
}
