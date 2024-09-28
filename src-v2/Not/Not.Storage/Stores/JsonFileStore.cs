using Not.Storage.Concurrency;
using Not.Storage.Ports;
using Not.Storage.Ports.States;
using System.Runtime.CompilerServices;

namespace Not.Storage.Stores;

public class JsonFileStore<T> : JsonStore<T>, IStore<T>
    where T : class, IState, new()
{
    private readonly ConcurrencySynchronizer _synchronizer;

    public JsonFileStore(IFileStorageConfiguration configuration) : base(Path.Combine(configuration.Path, $"{typeof(T).Name}.json"))
    {
        _synchronizer = new ConcurrencySynchronizer();
    }

    public async Task<T> Readonly([CallerFilePath] string callerPath = default!, [CallerMemberName] string callerMember = default!)
    {
        // TODO: figure out better way? Maybe cache the contents and update the cache on Commit?
        var transactionId = await _synchronizer.Wait(callerPath, callerMember);
        var state = await DeserializeAsync();
        _synchronizer.Release(transactionId);
        return state;
    }

    public async Task<T> Transact([CallerFilePath] string callerPath = default!, [CallerMemberName] string callerMember = default!)
    {
        var transactionId = await _synchronizer.Wait(callerPath, callerMember);

        var state = await DeserializeAsync();
        state.TransactionId = transactionId;
        return state;
    }

    public async Task Commit(T state)
    {
        if (state.TransactionId == null)
        {
            throw new Exception($"Cannot commit state without a transaction. Please use '{nameof(Transact)}' for write operations");
        }

        await SerializeAsync(state);
        _synchronizer.Release(state.TransactionId.Value);
    }
}