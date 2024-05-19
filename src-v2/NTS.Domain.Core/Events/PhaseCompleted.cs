using Not.Events;

namespace NTS.Domain.Core.Events;

public record PhaseCompleted : DomainObject, IEvent
{
    public PhaseCompleted(string tandemNumber, Person athlete, int loopNumber, double distance, Timestamp? outTime, bool isNotQualified)
    {
        TandemNumber = tandemNumber;
        Athlete = athlete;
        LoopNumber = loopNumber;
        Distance = distance;
        OutTime = outTime;
        IsNotQualified = isNotQualified;
    }

    public string TandemNumber { get; private set; }
    public Person Athlete { get; private set; }
    public int LoopNumber { get; private set; }
    public double Distance { get; private set; }
    public Timestamp? OutTime { get; private set; }
    public bool IsNotQualified { get; private set; }
}
