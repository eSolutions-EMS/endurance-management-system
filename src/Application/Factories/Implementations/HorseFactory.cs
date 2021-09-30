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

        public Horse Create(string feiId, string name, string breed, string club)
        {
            var horse = new Horse(feiId, name, breed, club);
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
