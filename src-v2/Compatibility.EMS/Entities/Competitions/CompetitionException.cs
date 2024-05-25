using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Competitions;

public class CompetitionException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Competition);
}
