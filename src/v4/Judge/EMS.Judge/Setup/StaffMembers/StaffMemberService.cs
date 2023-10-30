using Common.Application.CRUD;
using Common.Conventions;
using Common.Domain.Ports;
using EMS.Domain.Setup.Entities;
using EMS.Judge.Setup.Staff;

namespace EMS.Judge.Setup.StaffMembers;

// TODO: ChildBase service? We only need Read/Update here
public class StaffMemberService : CrudBase<StaffMember, StaffMemberCreateModel, StaffMemberUpdateModel>,
    IManageStaffMember
{
    private readonly IRepository<Event> events;

    public StaffMemberService(IRepository<StaffMember> staffMembers, IRepository<Event> events) : base(staffMembers)
    {
        this.events = events;
    }

    protected override StaffMember Build(StaffMemberCreateModel model)
    {
        return new StaffMember(model.Name!, model.Role);
    }
    protected override StaffMember Build(StaffMemberUpdateModel model)
    {
        return new StaffMember(model.Name, model.Role);
    }
    protected override StaffMemberUpdateModel BuildUpdateModel(StaffMember model)
    {
        return new StaffMemberUpdateModel(model);
    }
}

public interface IManageStaffMember : IUpdate<StaffMemberUpdateModel>, IRead<StaffMember>, ITransientService
{
}
