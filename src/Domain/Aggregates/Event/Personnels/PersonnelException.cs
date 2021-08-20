using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Event.Personnels
{
    public class PersonnelException : DomainException
    {
        protected override string Entity { get; } = nameof(Personnel);
    }
}
