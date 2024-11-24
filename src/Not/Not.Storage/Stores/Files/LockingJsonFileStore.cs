using System.Runtime.CompilerServices;
using Not.Concurrency;
using Not.Storage.States;
using Not.Storage.Stores.Files.Ports;

namespace Not.Storage.Stores.Files;

public class LockingJsonFileStore<T> : JsonFileStore<T>, IStore<T>
    where T : class, IState, new()
{
    readonly TimeoutLockSemaphore _timeoutLock;

    public LockingJsonFileStore(IFileStorageConfiguration configuration)
        : base(Path.Combine(configuration.Path, $"{typeof(T).Name}.json"))
    {
        _timeoutLock = new TimeoutLockSemaphore();
    }

    public async Task<T> Readonly(
        [CallerFilePath] string callerPath = default!,
        [CallerMemberName] string callerMember = default!
    )
    {
        // TODO: figure out better way? Maybe cache the contents and update the cache on Commit?
        var transactionId = await _timeoutLock.Wait(callerPath, callerMember);
        var state = await DeserializeAsync();
        _timeoutLock.Release(transactionId);
        return state;
    }

    public async Task<T> Transact(
        [CallerFilePath] string callerPath = default!,
        [CallerMemberName] string callerMember = default!
    )
    {
        var transactionId = await _timeoutLock.Wait(callerPath, callerMember);

        var state = await DeserializeAsync();
        state.TransactionId = transactionId;
        return state;
    }

    public async Task Commit(T state)
    {
        if (state.TransactionId == null)
        {
            throw new Exception(
                $"Cannot commit state without a transaction. Please use '{nameof(Transact)}' for write operations"
            );
        }

        await SerializeAsync(state);
        _timeoutLock.Release(state.TransactionId.Value);
    }
}
