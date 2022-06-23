using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Services;

public class WitnessPollingService : IWitnessPollingService
{
    private readonly CancellationTokenSource polling = new();
    private readonly ILogger logger;
    private readonly IDataService dataService;
    private readonly IWitnessEventQueue witnessEventQueue;
    private readonly IPersistence persistence;
    private bool isPolling;

    public WitnessPollingService(
        ILogger logger,
        IDataService dataService,
        IWitnessEventQueue witnessEventQueue,
        IPersistence persistence)
    {
        this.logger = logger;
        this.dataService = dataService;
        this.witnessEventQueue = witnessEventQueue;
        this.persistence = persistence;
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
                // TODO: check if initialized
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                this.AddEvents();
                await Task.Delay(5000, cancellationToken);
            }
            catch (Exception exception)
            {
                // this.persistence.SaveState();
                this.logger.LogEventError(exception);
                await Task.Delay(5000, cancellationToken);
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
            // TODO: remove queue and publish events directly
            this.logger.LogEvent(witnessEvent);
            this.witnessEventQueue.AddEvent(witnessEvent);
            this.ApplyEvents();
        }
    }
}

public interface IWitnessPollingService : IDisposable
{
    void ApplyEvents();
}
