using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Athletes;

public class EmsAthleteException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsAthlete);
}
