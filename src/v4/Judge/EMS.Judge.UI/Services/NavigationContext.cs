using Common.Conventions;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.UI.Services;

public class NavigationContext : INavigationContext
{
    public StaffMember StaffMemberUpdate { get; set; } = default!;
}

public interface INavigationContext : ISingletonService
{
    StaffMember StaffMemberUpdate { get; set; }
}
