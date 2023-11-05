using Common;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.StaffMembers;

public class OfficialUpdateModel : IIdentifiable
{
    public OfficialUpdateModel(Official member)
    {
        this.Id = member.Id;
        this.Name = member.Person;
        this.Role = member.Role;
    }

    public int Id { get; }
    public string Name { get; set; }
    public OfficialRole Role { get; set; }
}
