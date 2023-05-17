using Core.Domain.Enums;
using Core.Models;

namespace Core.Domain.State.Personnels;

public interface IPersonnelState : IIdentifiable
{
    public string Name { get; }

    public PersonnelRole Role { get; }
}
