using EMS.Core.Domain.Core.Exceptions;
using EMS.Core.Localization;

namespace EMS.Core.Domain.State.LapRecords;

public class LapRecordException : DomainExceptionBase
{
    protected override string Entity => Strings.LAP_RECORD_ENTITY;
}
