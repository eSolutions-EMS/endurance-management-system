using Not.Domain;
using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

public interface IReadBehind<T> : ISingleton
{
    Task<T?> Read(int id);
}
