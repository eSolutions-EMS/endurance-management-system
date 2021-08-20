using EnduranceJudge.Application.Import.ImportFromFile.Models;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Application.Factories
{
    public interface IAthleteFactory : IService
    {
        Athlete Create(HorseSportShowEntriesAthlete data);
        Athlete Create(IAthleteState data);
    }
}
