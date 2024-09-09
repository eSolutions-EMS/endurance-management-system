using NTS.Domain.Setup.Entities;


namespace NTS.Judge.Blazor.Pages.Setup.Horses;
public class HorseFormModel
{
    public HorseFormModel() 
    {
        // mock data for testing
        FeiId = "66";
        Name = "Хан Аспарух";
    }
    public HorseFormModel(Horse horse)
    {
        Id = horse.Id;
        FeiId = horse.FeiId;
        Name = horse.Name;
    }

    public int Id { get; set; }
    public string? FeiId { get; set; }
    public string Name { get; set; }
}
