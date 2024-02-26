using Newtonsoft.Json;
using JsonNet.PrivatePropertySetterResolver;
using Not.Injection;

namespace EMS.Persistence;

public class InMemoryJsonStore<T> : IStore<T>
    where T : class, new()
{
    private T? _context;
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
            if (_context != null)
            {
                return Task.FromResult(_context);
            }
            var state = new T();
            if (_json == null)
            {
                return Task.FromResult(state);
            }
            state = JsonConvert.DeserializeObject<T>(_json, _settings) ?? state;
            return Task.FromResult(state);
        }
    }

    public Task Commit(T state)
    {
        lock (_lock)
        {
            _json = JsonConvert.SerializeObject(state, _settings);
            _context = null;
            return Task.CompletedTask;
        }
    }
}

public interface IStore<T> : ISingletonService
    where T : class, new()
{
    internal Task<T> Load();
    internal Task Commit(T context);
}


