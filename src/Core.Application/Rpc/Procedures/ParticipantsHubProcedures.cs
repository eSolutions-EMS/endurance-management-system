using Core.Domain.AggregateRoots.Manager.Aggregates.ParticipantEntries;
using Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Rpc.Procedures;

public interface IParticipantstHubProcedures
{
    IEnumerable<ParticipantEntry> Get();
    Task Save(IEnumerable<ParticipantEntry> entries);
}
