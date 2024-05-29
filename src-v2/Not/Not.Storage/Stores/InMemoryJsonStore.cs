using Newtonsoft.Json;
using JsonNet.PrivatePropertySetterResolver;

namespace Not.Storage.Stores;

public class InMemoryJsonStore<T> : IStore<T>
    where T : class, new()
{
    private T _context = new();
    private string? _json;
    private readonly JsonSerializerSettings _settings;
    private readonly object _lock = new object();

    public InMemoryJsonStore()
    {
        _settings = new JsonSerializerSettings();
        _settings.ContractResolver = new PrivatePropertySetterResolver();
        _settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        _settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
    }

    public Task<T> Load()
    {
        lock (_lock)
        {
            return Task.FromResult(_context);
        }
    }

    public Task Commit(T state)
    {
        lock (_lock)
        {
            return Task.CompletedTask;
        }
    }
}