using EnduranceJudge.Application.Import.ImportFromFile.Models;
using EnduranceJudge.Domain.Aggregates.Common.Horses;

namespace EnduranceJudge.Application.Factories.Implementations
{
    public class HorseFactory : IHorseFactory
    {
        public Horse Create(HorseSportShowEntriesHorse data)
        {
            var hasParsed = bool.TryParse(data.Stallion, out var isStallion);
            if (!hasParsed)
            {
                return null;
            }

            var horse = new Horse(
                default,
                data.FEIID,
                data.Name,
                isStallion,
                data.StudBook,
                data.TrainerFEIID,
                data.TrainerFirstName,
                data.TrainerFamilyName);
            return horse;
        }

        public Horse Create(HorseForNationalImportModel data)
        {
            var horse = new Horse(data.FeiId, name: data.Name, breed: data.Breed);
            return horse;
        }

        public Horse Create(IHorseState data)
        {
            var horse = new Horse(
                data.Id,
                data.FeiId,
                data.Name,
                data.IsStallion,
                data.Breed,
                data.TrainerFeiId,
                data.TrainerFirstName,
                data.TrainerLastName);
            return horse;
        }
    }
}
