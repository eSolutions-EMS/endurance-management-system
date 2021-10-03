using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Athletes
{
    public class RiderException : DomainException
    {
        protected override string Entity { get; } = nameof(Athlete);
    }
}
