using EMS.Core.Domain.Enums;
using EMS.Core.Models;

namespace EMS.Core.Domain.State.Personnels;

public interface IPersonnelState : IIdentifiable
{
    public string Name { get; }

    public PersonnelRole Role { get; }
}
