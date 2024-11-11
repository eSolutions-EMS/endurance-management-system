using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

public interface IUpdateBehind<T> : ISingleton
{
    Task Update(T entity);
}
