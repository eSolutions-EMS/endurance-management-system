using Not;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsResultState : IIdentifiable
{
    bool IsNotQualified { get; }

    string Code { get; }
}
