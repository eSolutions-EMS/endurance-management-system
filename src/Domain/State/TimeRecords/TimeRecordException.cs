using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Localization;

namespace EnduranceJudge.Domain.State.TimeRecords;

public class TimeRecordException : DomainExceptionBase
{
    protected override string Entity => Strings.TIME_RECORD_ENTITY;
}
