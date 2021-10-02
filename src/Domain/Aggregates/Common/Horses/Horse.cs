using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Common.Horses
{
    public class Horse : DomainObjectBase<HorseException>, IHorseState, IAggregateRoot
    {
        private Horse()
        {
        }

        public Horse(string feiId, string name, string breed, string club): base(default)
        => this.Validate(() =>
        {
            this.Name = name.IsRequired(nameof(name));
            this.Breed = breed.IsRequired(nameof(breed));
            this.FeiId = feiId;
            this.Club = club;
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
                this.Name = name.IsRequired(nameof(name));
                this.Breed = breed.IsRequired(nameof(breed));
                this.IsStallion = isStallion;
                this.FeiId = feiId;
                this.TrainerFeiId = trainerFeiId;
                this.TrainerFirstName = trainerFirstName;
                this.TrainerLastName = trainerLastName;
            });

        public string FeiId { get; private set; }
        public string Name { get; private set; }
        public string Club { get; private set; }
        public bool IsStallion { get; private set; }
        public string Breed { get; private set; }
        public string TrainerFeiId { get; private set; }
        public string TrainerFirstName { get; private set; }
        public string TrainerLastName { get; private set; }
    }
}
