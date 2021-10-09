using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Personnels
{
    public class PersonnelObjectException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(Personnel);
    }
}
