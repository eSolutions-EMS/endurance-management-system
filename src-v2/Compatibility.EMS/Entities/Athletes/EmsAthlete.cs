using Core.Domain.State.Athletes;
using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Countries;
using NTS.Compatibility.EMS.Enums;

namespace NTS.Compatibility.EMS.Entities.Athletes;

public class EmsAthlete : EmsDomainBase<EmsAthleteException>
{
    private const int ADULT_AGE_IN_YEARS = 18;

    [Newtonsoft.Json.JsonConstructor]
    private EmsAthlete() { }

    public EmsAthlete(
        string feiId,
        string firstName,
        string lastName,
        EmsCountry country,
        DateTime birthDate
    )
        : base(GENERATE_ID)
    {
        this.FeiId = feiId;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Country = country;
        this.Category =
            birthDate.AddYears(ADULT_AGE_IN_YEARS) <= DateTime.Now
                ? EmsCategory.Seniors
                : EmsCategory.Children;
    }

    public EmsAthlete(IEmsAthleteState state, EmsCountry country)
        : base(GENERATE_ID)
    {
        this.FeiId = state.FeiId;
        this.Club = state.Club;
        this.FirstName = state.FirstName;
        this.LastName = state.LastName;
        this.Category = state.Category;
        this.Country = country;
    }

    public string FeiId { get; internal set; }
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public string Club { get; internal set; }
    public EmsCategory Category { get; internal set; }
    public EmsCountry Country { get; internal set; }

    public string Name => $"{this.FirstName} {this.LastName}";
}
