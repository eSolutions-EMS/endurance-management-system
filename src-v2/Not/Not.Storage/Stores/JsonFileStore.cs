using Not.Filesystem;
using Not.Serialization;
using Not.Storage.Concurrency;
using Not.Storage.Ports;
using Not.Storage.Ports.States;
using System.Runtime.CompilerServices;

namespace Not.Storage.Stores;

public class JsonFileStore<T> : IStore<T>
    where T : class, IState, new()
{
    private readonly ConcurrencySynchronizer _synchronizer;
    private readonly string _path;

    public JsonFileStore(IFileStorageConfiguration configuration)
    {
        _synchronizer = new ConcurrencySynchronizer();
        _path = Path.Combine(configuration.Path, $"{typeof(T).Name}.json"); ;
    }

    public async Task<T> Readonly()
    {
        return await Deserialize();
    }

    public async Task<T> Transact([CallerFilePath] string callerPath = default!, [CallerMemberName] string callerMember = default!)
    {
        var transactionId = await _synchronizer.Wait(callerPath, callerMember);

        var state = await Deserialize();
        state.TransactionId = transactionId;
        return state;
    }

    public async Task Commit(T state)
    {
        if (state.TransactionId == null)
        {
            throw new Exception($"Cannot commit state without a transaction. Please use '{nameof(Transact)}' for write operations");
        }

        var json = state.ToJson();
        await FileHelper.Write(_path, json);
        _synchronizer.Release(state.TransactionId.Value);
    }

    private async Task<T> Deserialize()
    {
        var contents = await FileHelper.SafeReadString(_path);
        return contents?.FromJson<T>() ?? new();
    }
}