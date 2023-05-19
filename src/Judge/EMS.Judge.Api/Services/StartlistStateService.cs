using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Judge.Api.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EMS.Judge.Api.Services;

public class StartlistStateService : IStartlistStateService
{
    private readonly ManagerRoot managerRoot;
    public StartlistStateService(IJudgeServiceProvider judgeServiceProvider)
    {
        this.managerRoot = judgeServiceProvider.GetRequiredService<ManagerRoot>();
    }

    public IEnumerable<StartModel> Get()
        => this.managerRoot.GetStartList(false);
}

public interface IStartlistStateService
{
    IEnumerable<StartModel> Get();
}
