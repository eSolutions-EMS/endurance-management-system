using Not.Events;

namespace NTS.Domain.Core.Events;

public record PhaseCompleted : DomainObject, IEvent
{
    public PhaseCompleted(int number)
    {
        Number = number;
    }

    public int Number { get; }
}
