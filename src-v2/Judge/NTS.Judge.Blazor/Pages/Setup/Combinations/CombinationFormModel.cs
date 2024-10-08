using NTS.Domain.Setup.Entities;


namespace NTS.Judge.Blazor.Pages.Setup.Combinations;
public class CombinationFormModel
{
    public CombinationFormModel()
    {
    }
    public CombinationFormModel(Combination combination)
    {
        Id = combination.Id;
        Number = combination.Number;
        Athlete = combination.Athlete;
        Horse = combination.Horse;
        Tag = combination.Tag;
    }

    public int Id { get; set; }
    public int Number { get; set; }
    public Athlete? Athlete { get; set; }
    public Horse? Horse { get; set;}
    public Tag? Tag { get; set; }
}
