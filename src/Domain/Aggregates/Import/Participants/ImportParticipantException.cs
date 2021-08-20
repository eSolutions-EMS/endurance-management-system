using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Import.Participants
{
    public class ImportParticipantException : DomainException
    {
        protected override string Entity { get; } = $"Import {nameof(Participant)}";
    }
}
