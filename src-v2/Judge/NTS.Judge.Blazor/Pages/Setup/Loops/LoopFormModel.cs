using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Pages.Setup.Loops;
public class LoopFormModel
{
    public LoopFormModel()
    {
        // mock data for testing
        Distance = 20;
    }
    public LoopFormModel(Loop phase)
    {
        Id = phase.Id;
        Distance = phase.Distance;

    }

    public int Id { get; set; }
    public double Distance { get; set; }
}
