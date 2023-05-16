using EMS.Core.Models;

namespace EMS.Core.Domain.State.Participants;

public interface IParticipantState : IIdentifiable
{
    public string RfIdHead { get; }
    public string RfIdNeck { get; }
    public string Number { get; }
    int? MaxAverageSpeedInKmPh { get; }
}
