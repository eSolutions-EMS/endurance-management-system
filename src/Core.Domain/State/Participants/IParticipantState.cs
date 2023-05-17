using Core.Models;

namespace Core.Domain.State.Participants;

public interface IParticipantState : IIdentifiable
{
    public string RfIdHead { get; }
    public string RfIdNeck { get; }
    public string Number { get; }
    int? MaxAverageSpeedInKmPh { get; }
}
