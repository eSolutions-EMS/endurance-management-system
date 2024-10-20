using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Events.Participations;

public record ParticipationRestored : ParticipationEventBase
{
    public ParticipationRestored(Participation participation) : base(participation)
    {
    }
}
