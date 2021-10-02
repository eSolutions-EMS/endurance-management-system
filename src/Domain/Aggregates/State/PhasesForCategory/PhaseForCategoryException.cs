using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.State.PhasesForCategory
{
    public class PhaseForCategoryException : DomainException
    {
        protected override string Entity { get; } = nameof(PhaseForCategory);
    }
}
