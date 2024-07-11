using NTS.Compabitility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Participants;

public interface IEmsParticipantState : IEmsIdentifiable
{
    public bool Unranked { get; }
    public string Number { get; }
    int? MaxAverageSpeedInKmPh { get; }
}
