using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.Officials;

public interface IOfficialFields
{
    string Name { get; set; }
    OfficialRole Role { get; set; }
}
