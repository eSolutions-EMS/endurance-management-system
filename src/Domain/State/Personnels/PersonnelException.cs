using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Personnels
{
    public class PersonnelException : DomainException
    {
        protected override string Entity { get; } = nameof(Personnel);
    }
}
