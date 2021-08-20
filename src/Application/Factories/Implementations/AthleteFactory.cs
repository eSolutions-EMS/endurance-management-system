using EnduranceJudge.Application.Import.ImportFromFile.Models;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Domain.States;
using System;
using System.Globalization;

namespace EnduranceJudge.Application.Factories.Implementations
{
    public class AthleteFactory : IAthleteFactory
    {
        public Athlete Create(HorseSportShowEntriesAthlete data)
        {
            var hasParsed = DateTime.TryParseExact(
                data.BirthDate,
                "yyyy-mm-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var birthDate);
            if (!hasParsed)
            {
                return null;
            }

            var athlete = new Athlete(
                default,
                data.FEIID,
                data.FirstName,
                data.FamilyName,
                data.CompetingFor,
                birthDate);
            return athlete;
        }

        public Athlete Create(IAthleteState data)
        {
            var athlete = new Athlete(
                data.Id,
                data.FeiId,
                data.FirstName,
                data.LastName,
                data.CountryIsoCode,
                data.Category);
            return athlete;
        }
    }
}
