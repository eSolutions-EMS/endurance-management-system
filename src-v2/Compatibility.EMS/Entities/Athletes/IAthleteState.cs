using Core.Domain.Enums;
using NTS.Compabitility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Athletes;

public interface IAthleteState : IIdentifiable
{
    public string FeiId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Club { get; }
    public Category Category { get; }
}
