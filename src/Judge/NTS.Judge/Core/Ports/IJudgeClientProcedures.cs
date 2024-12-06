using NTS.Domain.Objects;

namespace NTS.Judge.Core.Ports;

public interface IJudgeClientProcedures
{
    Task Process(IEnumerable<Snapshot> snapshots);
}
