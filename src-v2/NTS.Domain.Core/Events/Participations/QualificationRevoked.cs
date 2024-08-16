using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Events.Participations;

public record QualificationRevoked : ParticipationEventBase
{
    public QualificationRevoked(Participation participation) : base(participation)
    {
    }
}
