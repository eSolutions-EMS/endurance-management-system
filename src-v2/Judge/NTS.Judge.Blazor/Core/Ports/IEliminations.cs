using NTS.Domain.Core.Entities.ParticipationAggregate;

namespace NTS.Judge.Blazor.Core.Ports;

public interface IEliminations : IParticipationContext
{
    Task Withdraw();
    Task Retire();
    Task FinishNotRanked(string reason);
    Task Disqualify(string reason);
    Task FailToQualify(FtqCode[] ftqCodes, string? reason);
    Task RestoreQualification();
}