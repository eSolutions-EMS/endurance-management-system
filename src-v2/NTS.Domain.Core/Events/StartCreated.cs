using Not.Events;

namespace NTS.Domain.Core.Events;

public record StartCreated : DomainObject, IEvent
{
    public StartCreated(string number, Person athlete, int loopNumber, double distance, Timestamp startAt)
    {
        Number = number;
        Athlete = athlete;
        LoopNumber = loopNumber;
        Distance = distance;
        StartAt = startAt;
    }

    public string Number { get; }
    public Person Athlete { get; }
    public int LoopNumber { get; }
    public double Distance { get; }
    public Timestamp StartAt { get; }
}
