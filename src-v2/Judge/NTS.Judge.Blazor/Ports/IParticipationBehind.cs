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
    Participation? SelectedParticipation { get; }
    void SelectParticipation(int number);
    Task Process(Snapshot snapshot);
    Task Update(IPhaseState state);
    Task Update(Participation participation);
    Task RevokeQualification(int number, QualificationRevokeType type, string? reason = null, params FTQCodes[] ftqCodes);
    Task RestoreQualification(int number);
    Task<Participation?> Get(int id);
}
