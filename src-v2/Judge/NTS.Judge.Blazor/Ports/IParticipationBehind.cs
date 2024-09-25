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
    Task Process(Snapshot snapshot);
    Task Update(IPhaseState state);
    Task Withdraw(int number);
    Task Retire(int number);
    Task FinishNotRanked(int number, string reason);
    Task Disqualify(int number, string reason);
    Task FailToQualify(int number, FTQCodes[] ftqCodes);
    Task FailToCompleteLoop(int number, string? reason, FTQCodes[] ftqCodes);
    Task RestoreQualification(int number);
    Task<Participation?> Get(int id);
}
