using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Judge.Api.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EMS.Judge.Api.Services;

public class StartlistService : IStartlistService
{
    private readonly ManagerRoot managerRoot;
    public StartlistService(IJudgeServiceProvider judgeServiceProvider)
    {
        this.managerRoot = judgeServiceProvider.GetRequiredService<ManagerRoot>();
    }

    public Dictionary<int, Startlist> Get()
        => this.managerRoot.GetStartList();
}

public interface IStartlistService
{
    Dictionary<int, Startlist> Get();
}
