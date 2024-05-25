using Not.Injection;
using NTS.Domain.Core.Events;
using NTS.Domain.Core.Objects;
using NTS.Domain.Objects;

namespace NTS.Judge.Ports;

public interface IRemoteProcedures : ITransientService
{
    Task ReceiveSnapshot(Snapshot snapshot);
    Task SendStartCreated(PhaseStart startCreated);
    Task SendQualificationRevoked(QualificationRevoked revoked);
    Task SendQualificationRestored(QualificationRestored restored);
}
