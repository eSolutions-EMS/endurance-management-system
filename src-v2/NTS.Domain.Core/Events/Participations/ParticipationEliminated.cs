using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Events.Participations;

public record ParticipationEliminated : ParticipationEventBase
{
    public ParticipationEliminated(Participation participation) : base(participation)
    {
    }
}
