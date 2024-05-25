using Not.Serialization;

namespace Not.Storage.Stores;

public class InMemoryJsonStore<T> : IStore<T>
    where T : class, new()
{
    private T? _context;
    private string? _json;
    private readonly object _lock = new();

    public Task<T> Load()
    {
        lock (_lock)
        {
            if (_context != null)
            {
                return Task.FromResult(_context);
            }
            //TODO: remove _context
            var state = new T();
            if (_json == null)
            {
                return Task.FromResult(state);
            }
            state = _json.FromJson<T>();
            return Task.FromResult(state);
        }
    }

    public Task Commit(T state)
    {
        lock (_lock)
        {
            _json = state.ToJson();
            _context = null;
            return Task.CompletedTask;
        }
    }
}