using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.LapRecords;

public class EmsLapRecordException : EmsDomainExceptionBase
{
    protected override string Entity => "Lap";
}
