using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.LapRecords;

public class LapRecordException : DomainExceptionBase
{
    protected override string Entity => "Lap";
}
