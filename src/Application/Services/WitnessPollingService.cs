using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Services;

public class WitnessPollingService : IWitnessPollingService
{
    private readonly CancellationTokenSource polling = new();
    private readonly IDataService dataService;
    private readonly IWitnessEventQueue witnessEventQueue;
    private bool isPolling;

    public WitnessPollingService(IDataService dataService, IWitnessEventQueue witnessEventQueue)
    {
        this.dataService = dataService;
        this.witnessEventQueue = witnessEventQueue;
    }

    public void ApplyEvents()
    {
        if (!this.isPolling)
        {
            this.StartPollingInBackground();
        }
        this.witnessEventQueue.ApplyEvents();
    }

    public void Dispose()
    {
        this.polling.Cancel();
        this.polling.Dispose();
    }

    private void StartPollingInBackground()
        => Task.Run(() => this.PollEventApi(this.polling.Token));
    
    private async Task PollEventApi(CancellationToken cancellationToken)
    {
        this.isPolling = true;
        while (true)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                this.AddEvents();
                await Task.Delay(5000, cancellationToken);
            }
            catch (Exception exception)
            {
                // TODO: error hadnling
                this.isPolling = false;
                break;
            }
        }
    }

    private void AddEvents()
    {
        var eventsByIndex = this.dataService.Get();
        var events = eventsByIndex
            .OrderBy(x => x.Key)
            .Select(x => x.Value);
        foreach (var witnessEvent in events)
        {
            this.witnessEventQueue.AddEvent(witnessEvent);
        }
    }
}

public interface IWitnessPollingService : IDisposable
{
    void ApplyEvents();
}
