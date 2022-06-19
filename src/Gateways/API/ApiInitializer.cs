using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Core.Utilities;
using System;

namespace Endurance.Judge.Gateways.API
{
    public class ApiInitializer : IInitializer
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IStateManager stateManager;

        public ApiInitializer(IServiceProvider serviceProvider, IStateManager stateManager)
        {
            this.serviceProvider = serviceProvider;
            this.stateManager = stateManager;
        }

        public int RunningOrder => 20;
        public void Run()
        {
            this.stateManager.Load();
            StaticProvider.Initialize(this.serviceProvider);
        }
    }
}
