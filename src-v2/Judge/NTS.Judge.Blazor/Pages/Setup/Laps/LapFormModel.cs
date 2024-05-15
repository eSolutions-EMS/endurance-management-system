using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Pages.Setup.Laps;
public class LapFormModel
{
    public LapFormModel()
    {
        // mock data for testing
        Distance = 20;
    }
    public LapFormModel(Lap lap)
    {
        Id = lap.Id;
        Distance = lap.Distance;
    }

    public int Id { get; set; }
    public double Distance { get; set; }
}
