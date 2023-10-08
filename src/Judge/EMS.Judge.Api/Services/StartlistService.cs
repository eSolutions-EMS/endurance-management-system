using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Judge.Api.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Api.Services;

public class StartlistService : IStartlistService
{
    private readonly ManagerRoot managerRoot;
    public StartlistService(IJudgeServiceProvider judgeServiceProvider)
    {
        this.managerRoot = judgeServiceProvider.GetRequiredService<ManagerRoot>();
    }

    public IEnumerable<StartlistEntry> Get()
        => Enumerable.Empty<StartlistEntry>();
        //=> this.managerRoot.GetStartList();
}

public interface IStartlistService
{
    IEnumerable<StartlistEntry> Get();
}
