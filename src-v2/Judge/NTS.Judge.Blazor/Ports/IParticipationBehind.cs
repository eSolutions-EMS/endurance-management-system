using Not.Blazor.Ports.Behinds;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IParticipationBehind : IObservableBehind
{
    // TODO: this should probably be removed and Participations can be returned from Start instead
    IEnumerable<Participation> Participations { get; }
    Participation? SelectedParticipation { get; set; }
    void SelectParticipation(int number);
    void RequestReinspection(bool requestFlag);
    void RequestRequiredInspection(bool requestFlag);
    Task Update(IPhaseState state);
    Task Withdraw();
    Task Retire();
    Task FinishNotRanked(string reason);
    Task Disqualify(string reason);
    Task FailToQualify(string? reason, FTQCodes[] ftqCodes);
    Task RestoreQualification();
    Task<Participation?> Get(int id);
}
