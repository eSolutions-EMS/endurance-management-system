using Core.Domain.Enums;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsAthlete : EmsDomainBase<EmsAthleteException>, IEmsAthleteState
{
    private const int ADULT_AGE_IN_YEARS = 18;

    private EmsAthlete() { }
    public EmsAthlete(string feiId, string firstName, string lastName, EmsCountry country, DateTime birthDate)
        : base(default)
    {
        FeiId = feiId;
        FirstName = firstName;
        LastName = lastName;
        Country = country;
        Category = birthDate.AddYears(ADULT_AGE_IN_YEARS) <= DateTime.Now
            ? EmsCategory.Seniors
            : EmsCategory.Children;
    }
    public EmsAthlete(IEmsAthleteState state, EmsCountry country) : base(default)
    {
        FeiId = state.FeiId;
        Club = state.Club;
        FirstName = state.FirstName;
        LastName = state.LastName;
        Category = state.Category;
        Country = country;
    }

    public string FeiId { get; internal set; }
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public string Club { get; internal set; }
    public EmsCategory Category { get; internal set; }
    public EmsCountry Country { get; internal set; }

    public string Name => $"{FirstName} {LastName}";
}
