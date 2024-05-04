using Not.Events;

namespace NTS.Domain.Core.Events;

internal class PhaseCompletedEvent
{
    private static Event<PhaseCompletedEvent> _event = new();
    public static void Emit(string tandemNumber, Person athlete, int loopNumber, double distance, DateTimeOffset? outTime, bool isNotQualified)
    {
        _event.Emit(new PhaseCompletedEvent(tandemNumber, athlete, loopNumber, distance, outTime, isNotQualified));
    }
    public static void Subscribe(Action<PhaseCompletedEvent> action)
    {
        _event.Subscribe(action);
    }

    private PhaseCompletedEvent(string tandemNumber, Person athlete, int loopNumber, double distance, DateTimeOffset? outTime, bool isNotQualified)
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
    public DateTimeOffset? OutTime { get; private set; }
    public bool IsNotQualified { get; private set; }
}
