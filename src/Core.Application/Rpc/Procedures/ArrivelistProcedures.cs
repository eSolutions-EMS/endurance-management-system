using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Rpc.Procedures;

public interface IArrivelistClientProcedures
{
    Task Update(ArrivelistEntry entry, CollectionAction action);
}

public interface IArrivelistHubProcedures
{
    IEnumerable<ArrivelistEntry> Get();
    Task Save(IEnumerable<ArrivelistEntry> entries);
}
