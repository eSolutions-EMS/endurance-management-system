using Core.Domain.Core.Exceptions;
using Core.Localization;

namespace Core.Domain.State.LapRecords;

public class LapRecordException : DomainExceptionBase
{
    protected override string Entity => Strings.LAP_RECORD_ENTITY;
}
