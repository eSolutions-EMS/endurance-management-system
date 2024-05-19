using Not.Injection;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Ports;


public interface IParticipationBehind : ITransientService
{
    Task Process(Snapshot snapshot);
    Task Update(IPhaseState state);
    Task RevokeQualification(int number, RevokeType type, FTQCodes? ftqCodes = null, string? reason = null);
    Task RestoreQualification(int number);
    Task CreateStart(int number);
}
