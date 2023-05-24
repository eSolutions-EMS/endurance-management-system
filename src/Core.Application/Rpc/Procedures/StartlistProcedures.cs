using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System.Collections.Generic;

namespace Core.Application.Rpc.Procedures;

public interface IStartlistClientProcedures
{
    void AddEntry(StartModel entry);
}

public interface IStartlistHubProcedures
{
    IEnumerable<StartModel> Get();
}
