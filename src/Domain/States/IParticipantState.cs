using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.States
{
    public interface IParticipantState : IDomainModelState
    {
        public string RfId { get; }
        public int Number { get; }
        int? MaxAverageSpeedInKmPh { get; }
    }
}
