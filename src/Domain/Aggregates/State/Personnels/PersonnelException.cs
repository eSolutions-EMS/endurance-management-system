using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.State.Personnels
{
    public class PersonnelException : DomainException
    {
        protected override string Entity { get; } = nameof(Personnel);
    }
}
