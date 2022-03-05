using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Personnels
{
    public class PersonnelException : DomainExceptionBase
    {
        protected override string Entity { get; } = nameof(Personnel);
    }
}
