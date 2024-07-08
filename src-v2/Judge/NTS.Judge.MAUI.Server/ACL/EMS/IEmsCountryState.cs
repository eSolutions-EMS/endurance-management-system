using Not;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsCountryState : IIdentifiable
{
    string IsoCode { get; }
    string Name { get; }
}
