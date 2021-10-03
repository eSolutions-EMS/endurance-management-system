using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State
{
    public class EventConfigurationException : DomainException
    {
        private static readonly string Name = nameof(EventState);

        protected override string Entity => Name;
    }
}
