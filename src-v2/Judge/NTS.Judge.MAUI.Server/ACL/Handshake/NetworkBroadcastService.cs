using Core.Application.Services;
using NTS.Judge.MAUI.Server;

namespace EMS.Judge.Api.Services;

public class NetworkBroadcastService : BackgroundService
{
    private readonly INetworkBroadcastService networkService;

    public NetworkBroadcastService(IJudgeServiceProvider judgeServiceProvider)
    {
        this.networkService = judgeServiceProvider.GetRequiredService<INetworkBroadcastService>();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Broadcasting");
        try
        {
            return this.networkService.StartBroadcasting(stoppingToken);
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
