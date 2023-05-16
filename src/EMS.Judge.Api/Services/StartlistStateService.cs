using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System.Collections.Generic;
using System.Linq;

namespace Endurance.Judge.Gateways.API.Services
{
    public class StartlistStateService : IStartlistStateService
    {
        private static List<StartModel> startlist = new();

        public void Set(IEnumerable<StartModel> startModels)
            => startlist = startModels.ToList();

        public List<StartModel> Get()
            => startlist.ToList();
    }

    public interface IStartlistStateService : ITransientService
    {
        void Set(IEnumerable<StartModel> startModels);
        List<StartModel> Get();
    }
}
