using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Import.EnduranceEvents
{
    public class EnduranceEventException : DomainException
    {
        protected override string Entity { get; } = $"Import {nameof(EnduranceEvent)}";
    }
}
