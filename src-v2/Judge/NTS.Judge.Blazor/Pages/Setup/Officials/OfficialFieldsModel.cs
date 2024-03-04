using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Setup.StaffMembers;

public class OfficialFieldsModel
{
    public OfficialFieldsModel()
    {
    }
    public OfficialFieldsModel(Official member)
    {
        this.Id = member.Id;
        this.Name = member.Person;
        this.Role = member.Role;
    }

    public int? Id { get; }
    public string? Name { get; set; }
    public OfficialRole Role { get; set; } = OfficialRole.Steward;
}
