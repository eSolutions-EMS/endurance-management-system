using Not.Injection;
using NTS.Domain.Core.Events;
using NTS.Domain.Objects;

namespace NTS.Judge.Ports;

public interface IRemoteProcedures : ITransientService
{
    Task ReceiveSnapshot(Snapshot snapshot);
    Task SendStartCreated(StartCreated startCreated);
    Task SendQualificationRevoked(QualificationRevoked revoked);
    Task SendQualificationRestored(QualificationRestored restored);
}
