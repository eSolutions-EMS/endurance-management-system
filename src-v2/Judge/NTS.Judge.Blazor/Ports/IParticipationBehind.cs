using Not.Blazor.Ports.Behinds;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Enums;

namespace NTS.Judge.Blazor.Ports;

public interface IParticipationBehind : IObservableBehind
{
    // TODO: this should probably be removed and Participations can be returned from Start instead
    IEnumerable<Participation> Participations { get; }
    IEnumerable<IGrouping<double, Participation>> ParticipationsByDistance { get; }
    Task Process(Snapshot snapshot);
    Task Update(IPhaseState state);
    Task RevokeQualification(int number, QualificationRevokeType type, FTQCodes? ftqCodes = null, string? reason = null);
    Task RestoreQualification(int number);
    Task<Participation?> Get(int id);
}
