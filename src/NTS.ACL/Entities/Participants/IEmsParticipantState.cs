using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Participants;

public interface IEmsParticipantState : IEmsIdentifiable
{
    public bool Unranked { get; }
    public string Number { get; }
    int? MaxAverageSpeedInKmPh { get; }
}
