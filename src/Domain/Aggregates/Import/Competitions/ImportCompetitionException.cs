using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Import.Competitions
{
    public class ImportCompetitionException : DomainException
    {
        protected override string Entity { get; } = $"Import {nameof(Competition)}";
    }
}
