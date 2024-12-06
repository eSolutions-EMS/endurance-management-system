using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Competitions;

public class EmsCompetitionException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsCompetition);
}
