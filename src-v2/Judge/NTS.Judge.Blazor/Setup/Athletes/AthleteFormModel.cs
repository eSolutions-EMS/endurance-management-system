using Not.Blazor.CRUD.Forms.Ports;
using NTS.Domain.Core.StaticOptions;
using NTS.Domain.Objects;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Setup.Athletes;

public class AthleteFormModel : IFormModel<Athlete>
{
    public AthleteFormModel()
    {
#if DEBUG
        Name = "Gucci Petrov";
        Club = "Конярче ООД";
#endif
        Country = StaticOption.SelectedCountry;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? FeiId { get; set; }
    public Country? Country { get; set; }
    public string? Club { get; set; }
    public AthleteCategory Category { get; set; } = AthleteCategory.Senior;

    public void FromEntity(Athlete athlete)
    {
        Id = athlete.Id;
        Name = athlete.Person;
        FeiId = athlete.FeiId;
        Country = athlete.Country;
        Club = athlete.Club.ToString();
        Category = athlete.Category;
    }
}
