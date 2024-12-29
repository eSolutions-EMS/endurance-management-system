using NTS.Domain.Objects;

namespace NTS.Application.RPC;

public interface IJudgeClientProcedures
{
    Task ReceiveSnapshots(IEnumerable<Snapshot> snapshots);
}
