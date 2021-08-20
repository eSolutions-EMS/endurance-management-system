using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Common.Athletes
{
    public class RiderException : DomainException
    {
        protected override string Entity { get; } = nameof(Athlete);
    }
}
