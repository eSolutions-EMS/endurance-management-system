using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.Participants;

public interface IParticipantState : IIdentifiable
{
    public string RfId { get; }
    public int Number { get; }
    int? MaxAverageSpeedInKmPh { get; }
}