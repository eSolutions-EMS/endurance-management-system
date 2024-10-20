using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Events.Participations;

public record QualificationRestored : ParticipationEventBase
{
    public QualificationRestored(Participation participation) : base(participation)
    {
    }
}
