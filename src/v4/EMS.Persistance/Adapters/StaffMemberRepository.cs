using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

// TODO: fix service registration generic isn't necessary on the typed implementation
public class StaffMemberRepository : RepositoryBase<StaffMember>
{
    public StaffMemberRepository()
    {
        //this.Data.Add((T)new Event("place", new Domain.Objects.Country("bg", "Bulgaria")));
    }
}
