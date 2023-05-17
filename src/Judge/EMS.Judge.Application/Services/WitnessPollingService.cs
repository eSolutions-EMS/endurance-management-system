using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMS.Judge.Application.Services;

public class WitnessPollingService : IWitnessPollingService
{
    private int FREQUENCY_MS = 5000;
    private readonly CancellationTokenSource polling = new();
    private readonly ISettings settings;
    private readonly ILogger logger;
    private readonly IDataService dataService;
    private readonly IWitnessEventQueue witnessEventQueue;
    private bool isPolling;

    public WitnessPollingService(
        ISettings settings,
        ILogger logger,
        IDataService dataService,
        IWitnessEventQueue witnessEventQueue)
    {
        this.settings = settings;
        this.logger = logger;
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
            if (!this.settings.IsConfigured)
            {
                continue;
            }
            if (this.settings.IsSandboxMode || cancellationToken.IsCancellationRequested)
            {
                break;
            }
            try
            {
                await Task.Run(this.Execute, cancellationToken);
            }
            catch (Exception exception)
            {
                // We don't want to break the loop even if logging fails
                // perhaps add another type of log here later on
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
                await Task.Delay(FREQUENCY_MS, cancellationToken);
            }
        }
    }

    private async Task Execute()
    {
        try
        {
            // TODO: check if initialized
            await this.AddEvents();
            await Task.Delay(FREQUENCY_MS);
        }
        catch (Exception exception)
        {
            // this.persistence.SaveState();
            this.logger.LogEventError(exception);
            await Task.Delay(FREQUENCY_MS);
        }
    }

    private async Task AddEvents()
    {
        var eventsByIndex = await this.dataService.GetWitnessEvents();
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
