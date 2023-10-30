using Common;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.StaffMembers;

public class StaffMemberUpdateModel : IIdentifiable
{
    public StaffMemberUpdateModel(StaffMember member)
    {
        this.Id = member.Id;
        this.Name = member.Person;
        this.Role = member.Role;
    }

    public int Id { get; }
    public string Name { get; set; }
    public StaffRole Role { get; set; }
}
