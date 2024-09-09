using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Events.Participations;

public record QualificationRestored : ParticipationEventBase
{
    public QualificationRestored(Participation participation) : base(participation)
    {
    }
}
