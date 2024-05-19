using Not.Events;

namespace NTS.Domain.Core.Events;

public record PhaseCompleted : DomainObject, IEvent
{
    public PhaseCompleted(int tandemNumber, Person athlete, int loopNumber, double distance, Timestamp? outTime)
    {
        TandemNumber = tandemNumber;
        Athlete = athlete;
        LoopNumber = loopNumber;
        Distance = distance;
        OutTime = outTime;
    }

    public int TandemNumber { get; private set; }
    public Person Athlete { get; private set; }
    public int LoopNumber { get; private set; }
    public double Distance { get; private set; }
    public Timestamp? OutTime { get; private set; }
}
