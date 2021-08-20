using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents
{
    public class EnduranceEventException : DomainException
    {
        private static readonly string Name = nameof(EnduranceEvent);

        protected override string Entity => Name;
    }
}
