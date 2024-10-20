using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Events.Participations;

public record QualificationRevoked : ParticipationEventBase
{
    public QualificationRevoked(Participation participation) : base(participation)
    {
    }
}
