using Not.Events;

namespace NTS.Domain.Core.Events;

public record StartCreated : DomainObject, IEvent
{
    public StartCreated(int number, Person athlete, int loopNumber, double distance, Timestamp startAt)
    {
        Number = number;
        Athlete = athlete;
        LoopNumber = loopNumber;
        Distance = distance;
        StartAt = startAt;
    }

    public int Number { get; private set; }
    public Person Athlete { get; private set; }
    public int LoopNumber { get; private set; }
    public double Distance { get; private set; }
    public Timestamp StartAt { get; private set; }
}
