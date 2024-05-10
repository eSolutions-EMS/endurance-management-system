using Not.Injection;

namespace Not.Storage;

public interface IStore<T> : ISingletonService
    where T : class, new()
{
    public Task<T> Load();
    public Task Commit(T context);
}