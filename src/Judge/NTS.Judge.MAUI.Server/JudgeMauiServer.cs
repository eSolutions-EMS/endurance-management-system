using EMS.Judge.Api.Services;
using Not.Concurrency;
using NTS.Judge.MAUI.Server.ACL;
using NTS.Judge.MAUI.Server.ACL.Handshake;

namespace NTS.Judge.MAUI.Server;

public class JudgeMauiServer
{
    public static void ConfigurePrentContainer(IServiceCollection services)
    {
        services.AddMauiServerServices();
    }

    public static Task Start(IServiceProvider callerProvider)
    {
        return TaskHelper.Run(() => { StartServer(callerProvider); return Task.CompletedTask; });
    }

    private static void StartServer(IServiceProvider callerProvider)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<IJudgeServiceProvider>(new JudgeServiceProvider(callerProvider));
        builder.Services.AddSignalR();
        builder.Services.AddHostedService<NetworkBroadcastService>();

        var app = builder.Build();

        app.Urls.Add("http://*:11337");

        Console.WriteLine("Starting Judge server...");

        app.MapHub<EmsRpcHub>(Constants.RPC_ENDPOINT);

        app.Run();
    }
}
