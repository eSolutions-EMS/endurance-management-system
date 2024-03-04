using Not.Injection;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Services;

public class NavigationContext : INavigationContext
{
    public Official OfficialUpdate { get; set; } = default!;
}

public interface INavigationContext : ISingletonService
{
    Official OfficialUpdate { get; set; }
}
