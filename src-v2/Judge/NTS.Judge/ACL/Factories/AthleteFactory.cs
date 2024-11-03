using NTS.Compatibility.EMS.Entities.Athletes;
using NTS.Compatibility.EMS.Entities.Countries;
using NTS.Compatibility.EMS.Enums;
using NTS.Domain.Core.Entities;
using NTS.Judge.ACL.Models;

namespace NTS.Judge.ACL.Factories;

public class AthleteFactory
{
    public static EmsAthlete Create(Participation participation)
    {
        var athleteState = new EmsAthleteState
        {
            Category = EmsCategory.Seniors, //TODO: after athlete
            Club = participation.Combination.Club?.ToString(),
            FeiId = "", //TODO: after athlete
            FirstName = participation.Combination.Name.ToString().Split().First(),
            LastName = participation.Combination.Name.ToString().Split().Last(),
            Id = participation.Combination.Id,
        };
        var country = new EmsCountry(
            participation.Combination.Country?.IsoCode ?? "iso",
            participation.Combination.Country?.Name ?? "country-name",
            1337
        );
        return new EmsAthlete(athleteState, country);
    }
}
