using Not.Events;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Events.Participations;

public abstract record ParticipationEventBase : DomainObject
{
    public ParticipationEventBase(Participation participation)
    {
        Participation = participation;
    }

    public Participation Participation { get; }
}
