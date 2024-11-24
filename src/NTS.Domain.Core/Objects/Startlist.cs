using NTS.Domain.Core.Aggregates;
using NTS.Domain.Core.Objects.Payloads;

namespace NTS.Domain.Core.Objects;

public class StartList
{
    static readonly TimeSpan START_EXPIRY_TIME = TimeSpan.FromMinutes(15);

    List<Start> _starts;

    public StartList(IEnumerable<Participation> participations, Action action)
    {
        _starts = [];
        foreach (var participation in participations)
        {
            if (!participation.IsEliminated())
            {
                var phases = participation.Phases;
                foreach (var phase in phases)
                {
                    if (phase.StartTime == null)
                    {
                        continue;
                    }
                    var phaseIndex = phases.IndexOf(phase);
                    var phaseNumber = phaseIndex + 1;
                    var start = new Start(
                        participation.Combination.Name,
                        participation.Combination.Number,
                        phaseNumber,
                        phases[phaseIndex].Length,
                        phase.StartTime.ToDateTimeOffset()
                    );
                    _starts.Add(start);
                }
            }
        }
        Participation.PHASE_COMPLETED_EVENT.Subscribe(PhaseCompletedHandler);
        ChangeHandler = action;
    }

    Action ChangeHandler { get; set; }

    public IReadOnlyList<Start> History
    {
        get
        {
            var history = _starts.Where(s => CurrentTime() - s.Time.TimeOfDay > START_EXPIRY_TIME);
            return OrderByTimeAndPhase(history);
        }
    }

    public IReadOnlyList<Start> Upcoming
    {
        get
        {
            var upcoming = _starts.Where(s =>
                CurrentTime() - s.Time.TimeOfDay <= START_EXPIRY_TIME
            );
            return OrderByTimeAndPhase(upcoming);
        }
    }

    public bool Any()
    {
        return _starts.Any();
    }

    void PhaseCompletedHandler(PhaseCompleted phaseCompleted)
    {
        var newStart = new Start(phaseCompleted.Participation);
        _starts.Add(newStart);
        ChangeHandler();
    }

    TimeSpan CurrentTime()
    {
        return DateTimeOffset.Now.TimeOfDay;
    }

    List<Start> OrderByTimeAndPhase(IEnumerable<Start> starts)
    {
        return starts.OrderBy(s => s.Time).ThenBy(s => s.PhaseNumber).ToList();
    }
}
