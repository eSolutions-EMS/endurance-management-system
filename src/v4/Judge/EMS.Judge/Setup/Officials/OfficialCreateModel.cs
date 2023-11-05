using EMS.Domain.Setup.Entities;
using System.ComponentModel.DataAnnotations;

namespace EMS.Judge.Setup.Staff;

public class OfficialCreateModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public OfficialRole Role { get; set; } = OfficialRole.Steward;
}
