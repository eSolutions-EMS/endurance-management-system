using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Localization;

namespace EnduranceJudge.Domain.State.TimeRecords;

public class LapRecordException : DomainExceptionBase
{
    protected override string Entity => Strings.LAP_RECORD_ENTITY;
}
