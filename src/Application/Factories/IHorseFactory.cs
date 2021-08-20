using EnduranceJudge.Application.Import.ImportFromFile.Models;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Common.Horses;

namespace EnduranceJudge.Application.Factories
{
    public interface IHorseFactory : IService
    {
        Horse Create(HorseSportShowEntriesHorse horse);

        Horse Create(HorseForNationalImportModel horse);

        Horse Create(IHorseState data);
    }
}
