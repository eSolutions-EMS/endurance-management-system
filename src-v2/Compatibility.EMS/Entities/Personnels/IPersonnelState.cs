using Core.Domain.Enums;
using NTS.Compabitility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Personnels;

public interface IPersonnelState : IIdentifiable
{
    public string Name { get; }

    public PersonnelRole Role { get; }
}
