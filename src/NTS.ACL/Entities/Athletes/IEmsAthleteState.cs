using NTS.ACL.Abstractions;
using NTS.ACL.Enums;

namespace Core.Domain.State.Athletes;

public interface IEmsAthleteState : IEmsIdentifiable
{
    public string FeiId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Club { get; }
    public EmsCategory Category { get; }
}
