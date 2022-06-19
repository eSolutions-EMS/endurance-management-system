using EnduranceJudge.Core.Services;
using EnduranceJudge.Core.Utilities;
using System;

namespace Endurance.Judge.Gateways.API
{
    public class ApiInitializer : IInitializer
    {
        private readonly IServiceProvider serviceProvider;
        
        public ApiInitializer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public int RunningOrder => 20;
        public void Run()
        {
            StaticProvider.Initialize(this.serviceProvider);
        }
    }
}
