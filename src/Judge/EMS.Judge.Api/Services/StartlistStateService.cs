using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Api.Services;

public class StartlistStateService : IStartlistStateService
{
    private static List<StartModel> startlist = new();

    public void Set(IEnumerable<StartModel> startModels)
        => startlist = startModels.ToList();

    public List<StartModel> Get()
        => startlist.ToList();
}

public interface IStartlistStateService
{
    void Set(IEnumerable<StartModel> startModels);
    List<StartModel> Get();
}
