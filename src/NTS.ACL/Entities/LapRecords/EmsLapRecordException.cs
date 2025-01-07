using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.LapRecords;

public class EmsLapRecordException : EmsDomainExceptionBase
{
    protected override string Entity => "Lap";
}
