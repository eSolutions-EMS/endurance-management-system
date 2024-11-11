using Core.Application.Services;
using NTS.Judge.MAUI.Server;

namespace NTS.Judge.MAUI.Server.ACL.Handshake;

public class NetworkBroadcastService : BackgroundService
{
    readonly INetworkBroadcastService networkService;

    public NetworkBroadcastService(IJudgeServiceProvider judgeServiceProvider)
    {
        networkService = judgeServiceProvider.GetRequiredService<INetworkBroadcastService>();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Broadcasting");
        try
        {
            return networkService.StartBroadcasting(stoppingToken);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error while broadcasting!");
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.StackTrace);
        }
        return Task.CompletedTask;
    }
}
