using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Pages.Setup.Loops;
public class LoopFormModel
{
    public LoopFormModel()
    {
        // mock data for testing
        Distance = 20;
    }
    public LoopFormModel(Loop Loop)
    {
        Id = Loop.Id;
        Distance = Loop.Distance;
    }

    public int Id { get; set; }
    public double Distance { get; set; }
}
