using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Countries;

namespace NTS.Compatibility.EMS.Entities.Athletes;

public class Athlete : DomainBase<AthleteException>
{
    private const int ADULT_AGE_IN_YEARS = 18;

    private Athlete() {}
    public Athlete(string feiId, string firstName, string lastName, Country country, DateTime birthDate)
        : base(GENERATE_ID)
    {
        this.FeiId = feiId;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Country = country;
        this.Category = birthDate.AddYears(ADULT_AGE_IN_YEARS) <= DateTime.Now
            ? Category.Seniors
            : Category.Children;
    }

    public string FeiId { get; internal set; }
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public string Club { get; internal set; }
    public Category Category { get; internal set; }
    public Country Country { get; internal set; }

    public string Name => $"{this.FirstName} {this.LastName}";
}
public enum Category
{
    Invalid = 0,
    Seniors = 1,
    Children = 2,
    JuniorOrYoungAdults = 3
}
