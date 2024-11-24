using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Competitions;

public class EmsCompetitionException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsCompetition);
}
