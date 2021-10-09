using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Participations
{
    public class ParticipationObjectException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(Participation);
    }
}
