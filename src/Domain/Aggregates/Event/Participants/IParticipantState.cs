using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Event.Participants
{
    public interface IParticipantState : IDomainModelState
    {
        public string RfId { get; }
        public int Number { get; }
        int? MaxAverageSpeedInKmPh { get; }
        int HorseId { get; }
        int AthleteId { get; }
    }
}
