using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

public class StaffMemberRepository : RepositoryBase<StaffMember>
{
    private readonly IState _state;
    
    public StaffMemberRepository(IState state)
    {
        _state = state;
    }

    public override Task<StaffMember?> Read(int id)
    {
        return Task.FromResult(_state.StaffMembers.FirstOrDefault(x => x.Id == id));
    }

    public override Task<StaffMember>  Update(StaffMember entity)
    {
        var existing = _state.StaffMembers.FirstOrDefault(x => x.Id == entity.Id);
        if (existing != null)
        {
            _state.StaffMembers.Remove(existing);    
        }
        _state.StaffMembers.Add(entity);
        return Task.FromResult(entity);
    }
}
