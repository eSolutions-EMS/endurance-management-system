using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Personnels;

public interface IEmsPersonnelState : IEmsIdentifiable
{
    public string Name { get; }

    public PersonnelRole Role { get; }
}
