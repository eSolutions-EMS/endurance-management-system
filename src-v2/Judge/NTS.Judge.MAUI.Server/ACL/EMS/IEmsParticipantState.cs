using Not;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsParticipantState : IIdentifiable
{
    public bool Unranked { get; }
    public string Number { get; }
    int? MaxAverageSpeedInKmPh { get; }
}
