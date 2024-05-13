using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Pages.Setup.Phases;
public class PhaseFormModel
{
    public PhaseFormModel()
    {
        // mock data for testing
        Distance = 20;
    }
    public PhaseFormModel(Phase phase)
    {
        Id = phase.Id;
        Distance = phase.Distance;

    }

    public int Id { get; set; }
    public double Distance { get; set; }
}
