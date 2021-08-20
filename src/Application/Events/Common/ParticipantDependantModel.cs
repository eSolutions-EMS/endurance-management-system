using EnduranceJudge.Domain.Aggregates.Event.Participants;

namespace EnduranceJudge.Application.Events.Common
{
    public class ParticipantDependantModel : IParticipantState
    {
        public int Id { get; set; }
        public string RfId { get; set; }
        public int Number { get; set; }
        public int? MaxAverageSpeedInKmPh { get; set; }
        public string Name { get; set; }
        public int AthleteId { get; set; }
        public int HorseId { get; set; }
        public int CategoryId { get; set; }
    }
}
