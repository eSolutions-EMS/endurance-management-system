using Not.Blazor.Ports.Behinds;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Entities.ParticipationAggregate;

namespace NTS.Judge.Blazor.Ports;

// TODO: break this monster in parts:
// - IParticipationSelector
// - IInspectionBehind
// - ISnapshotBehind
// - IEliminationBehind

public interface IParticipationBehind : IObservableBehind, ISnapshotProcessor
{
    // TODO: this should probably be removed and Participations can be returned from Start instead
    IEnumerable<Participation> Participations { get; }
    Participation? SelectedParticipation { get; set; }
    Task RequestRepresent(bool requestFlag);
    Task RequireInspection(bool requestFlag);
    Task Withdraw();
    Task Retire();
    Task FinishNotRanked(string reason);
    Task Disqualify(string reason);
    Task FailToQualify(FtqCode[] ftqCodes, string? reason);
    Task RestoreQualification();
    Participation? Get(int id);
}
