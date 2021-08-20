using EnduranceJudge.Domain.Aggregates.Common.Horses;

namespace EnduranceJudge.Application.Events.Models
{
    public class HorseRootModel : IHorseState
    {
        public int Id { get; set; }
        public string FeiId { get; set; }
        public string Name { get; set; }
        public bool IsStallion { get; set; }
        public string Breed { get; set; }
        public string TrainerFeiId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }
    }
}
