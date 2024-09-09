using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Judge.Blazor.Pages.Dashboard.Phases;

public class PhaseUpdateModel : IPhaseState
{
    public PhaseUpdateModel(IPhaseState state)
    {
        Id = state.Id;
        StartTime = state.StartTime;
        ArriveTime = state.ArriveTime;
        InspectTime = state.InspectTime;
        ReinspectTime = state.ReinspectTime;
    }

    public int Id { get; }
    public Timestamp? StartTime { get; set; }
    public Timestamp? ArriveTime { get; set; }
    public Timestamp? InspectTime { get; set; }
    public Timestamp? ReinspectTime { get; set; }
}
