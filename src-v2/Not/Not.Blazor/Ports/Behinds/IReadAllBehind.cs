using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

public interface IReadAllBehind<T> : ISingleton
{
    Task<IEnumerable<T>> GetAll();
}
