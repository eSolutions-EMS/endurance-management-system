using EMS.Domain.Setup.Entities;
using EMS.Judge.Setup.Officials;
using System.ComponentModel.DataAnnotations;

namespace EMS.Judge.Setup.Staff;

public class OfficialCreateModel : IOfficialFields
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public OfficialRole Role { get; set; } = OfficialRole.Steward;
}
