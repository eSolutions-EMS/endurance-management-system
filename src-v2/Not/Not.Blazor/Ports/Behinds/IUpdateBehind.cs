using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

public interface IUpdateBehind<T> : ISingletonService
{
    Task Update(T entity);
}
