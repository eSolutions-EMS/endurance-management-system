using NTS.Domain.Core.Entities.ParticipationAggregate;

namespace NTS.Judge.Blazor.Pages.Dashboard.Phases;

public class PhaseUpdateModel : IPhaseState
{
    public PhaseUpdateModel(IPhaseState state)
    {
        Id = state.Id;
        StartTime = state.StartTime;
        ArriveTime = state.ArriveTime;
        PresentTime = state.PresentTime;
        RepresentTime = state.RepresentTime;
    }

    public int Id { get; }
    public Timestamp? StartTime { get; set; }
    public Timestamp? ArriveTime { get; set; }
    public Timestamp? PresentTime { get; set; }
    public Timestamp? RepresentTime { get; set; }
}
