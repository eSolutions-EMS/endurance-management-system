using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Rpc.Procedures;

public interface IStartlistClientProcedures
{
    Task Update(StartModel entry, CollectionAction action);
}

public interface IStartlistHubProcedures
{
    IEnumerable<StartModel> Get();
}
