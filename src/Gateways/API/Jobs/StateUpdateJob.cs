using Endurance.Judge.Gateways.API.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endurance.Judge.Gateways.API.Jobs
{
    public class StateUpdateJob : BackgroundService
    {
        private readonly IStateChangesQueue stateChangesQueue;
        private readonly IStateManager stateManager;

        public StateUpdateJob(IStateChangesQueue stateChangesQueue, IStateManager stateManager)
        {
            this.stateChangesQueue = stateChangesQueue;
            this.stateManager = stateManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!this.stateChangesQueue.IsEmpty())
                {
                    var state = this.stateChangesQueue.Dequeue();
                    this.stateManager.Update(state);
                }
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
