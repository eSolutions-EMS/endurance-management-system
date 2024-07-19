using Not.Events;
using NTS.Domain.Core.Abstractions;

namespace NTS.Domain.Core.Events;

public record PhaseCompleted : DomainObject, IEvent, IParticipationIdentifier
{
    public PhaseCompleted(int number)
    {
        Number = number;
    }

    public int Number { get; }
}
