using Not.Events;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Events.Participations;

public record PhaseCompleted : ParticipationEventBase
{
    public PhaseCompleted(Participation participation) : base(participation)
    {
    }
}
