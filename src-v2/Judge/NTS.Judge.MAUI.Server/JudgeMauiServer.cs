using EMS.Judge.Api.Services;
using Not.Safe;
using NTS.Judge.MAUI.Server.ACL;
using NTS.Judge.MAUI.Server.ACL.Handshake;
using System.Net;

namespace NTS.Judge.MAUI.Server;

public class JudgeMauiServer
{
    public static void ConfigurePrentContainer(IServiceCollection services)
    {
        services.AddMauiServerServices();
    }

    public static Task Start(IServiceProvider callerProvider)
    {
        return SafeHelper.RunAsync(async () => { await StartServer(callerProvider); });
    }

    public static async Task StartServer(IServiceProvider callerProvider)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<IJudgeServiceProvider>(new JudgeServiceProvider(callerProvider));
        builder.Services.AddSignalR(x => {
            x.EnableDetailedErrors = true;
        });
        builder.Services.AddHostedService<NetworkBroadcastService>();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
        builder.Logging.AddConsole();
        builder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
        builder.Logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Loopback, 11337);
        });

        var app = builder.Build();
        app.Urls.Add("http://*:11337");

        Console.WriteLine("Starting Judge server...");

        app.MapHub<EmsRpcHub>(Constants.RPC_ENDPOINT, options =>
        {
            options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents;
        });

        await app.RunAsync();
    }
}
