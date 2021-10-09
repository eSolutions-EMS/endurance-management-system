using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Horses
{
    public class HorseObjectException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(Horse);
    }
}
