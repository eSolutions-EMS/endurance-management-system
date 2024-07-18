using Not.Injection;
using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Judge.Blazor.Ports;

public interface IDasboardBehind : ITransientService
{
    // TODO: this should probably be removed and Participations can be returned from Start instead
    Task<IEnumerable<Participation>> GetParticipations();
    Task Start();
}