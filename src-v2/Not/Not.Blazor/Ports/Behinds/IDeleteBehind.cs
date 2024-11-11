using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

public interface IDeleteBehind<T> : ISingleton
{
    Task Delete(T entity);
}
