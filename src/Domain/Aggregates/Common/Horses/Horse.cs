using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Common.Horses
{
    public class Horse : DomainBase<HorseException>, IHorseState, IAggregateRoot
    {
        private Horse()
        {
        }

        public Horse(string feiId, string name, string breed): base(default)
            => this.Validate(() =>
            {
                this.FeiId = feiId.IsRequired(nameof(feiId));
                this.Name = name.IsRequired(nameof(name));
                this.Breed = breed.IsRequired(nameof(breed));
            });

        public Horse(
            int id,
            string feiId,
            string name,
            bool isStallion,
            string breed,
            string trainerFeiId,
            string trainerFirstName,
            string trainerLastName) : base(id)
            => this.Validate(() =>
            {
                this.FeiId = feiId.IsRequired(nameof(feiId));
                this.Name = name.IsRequired(nameof(name));
                this.Breed = breed.IsRequired(nameof(breed));
                this.TrainerFeiId = trainerFeiId.IsRequired(nameof(trainerFeiId));
                this.TrainerFirstName = trainerFirstName.IsRequired(nameof(trainerFirstName));
                this.TrainerLastName = trainerLastName.IsRequired(nameof(trainerLastName));
                this.IsStallion = isStallion;
            });

        public string FeiId { get; private set; }
        public string Name { get; private set; }
        public bool IsStallion { get; private set; }
        public string Breed { get; private set; }
        public string TrainerFeiId { get; private set; }
        public string TrainerFirstName { get; private set; }
        public string TrainerLastName { get; private set; }
    }
}
