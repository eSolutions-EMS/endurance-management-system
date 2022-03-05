using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.EnduranceEvents
{
    public class EnduranceEventException : DomainExceptionBase
    {
        private static readonly string Name = nameof(EnduranceEvent);

        protected override string Entity => Name;
    }
}
