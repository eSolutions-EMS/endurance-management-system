using NTS.Compabitility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Participants;

public interface IParticipantState : IIdentifiable
{
    public string Number { get; }
    int? MaxAverageSpeedInKmPh { get; }
}
