using NTS.Domain.Objects;

namespace NTS.Application.RPC.Procedures;

public interface IJudgeClientProcedures
{
    Task Process(IEnumerable<Snapshot> snapshots);
}
