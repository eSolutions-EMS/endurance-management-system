using EMS.Domain.Setup.Entities;
using System.ComponentModel.DataAnnotations;

namespace EMS.Judge.Setup.Staff;

public class StaffMemberCreateModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public StaffRole Role { get; set; } = StaffRole.Steward;
}
