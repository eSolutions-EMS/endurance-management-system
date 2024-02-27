using Not.Injection;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Blazor.Services;

public class NavigationContext : INavigationContext
{
    public Official OfficialUpdate { get; set; } = default!;
}

public interface INavigationContext : ISingletonService
{
    Official OfficialUpdate { get; set; }
}
