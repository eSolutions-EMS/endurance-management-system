using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Personnels;

public interface IEmsPersonnelState : IEmsIdentifiable
{
    public string Name { get; }

    public PersonnelRole Role { get; }
}
