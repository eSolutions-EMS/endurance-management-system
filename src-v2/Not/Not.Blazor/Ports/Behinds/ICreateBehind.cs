using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

public interface ICreateBehind<T> : ISingletonService
{
    Task Create(T model);
}
