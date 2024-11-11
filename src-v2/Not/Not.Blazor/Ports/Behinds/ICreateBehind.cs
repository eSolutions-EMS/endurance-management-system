using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

public interface ICreateBehind<T> : ISingleton
{
    Task Create(T model);
}
