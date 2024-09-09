using Not.Injection;
using Not.Domain;

namespace Not.Blazor.Ports.Behinds;

public interface IReadBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T?> Read(int id);
}
