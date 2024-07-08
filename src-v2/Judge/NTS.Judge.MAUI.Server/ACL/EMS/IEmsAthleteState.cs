using Core.Domain.Enums;
using Not;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public interface IEmsAthleteState : IIdentifiable
{
    public string FeiId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Club { get; }
    public EmsCategory Category { get; }
}
