using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Application.Events.Common
{
    public class ParticipantDependantModel : IParticipantState
    {
        public int Id { get; set; }
        public string RfId { get; set; }
        public int Number { get; set; }
        public int? MaxAverageSpeedInKmPh { get; set; }
        public int AthleteId { get; set; }
        public string AthleteName { get; set; }
        public int HorseId { get; set; }
        public string HorseName { get; set; }
        public int CategoryId { get; set; }
    }
}
