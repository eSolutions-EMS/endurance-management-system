using Not.Events;
using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

// TODO: Probably rename to EventBehind. However we need to rename the Domain entity Event
// To EnduranceEvent to avoid name conflicts
public abstract class ObservableBehind : IObservableBehind
{
    readonly SemaphoreSlim _semaphore = new(1);
    bool _isInitialized;
    private readonly IEventManager _stateChanged = new EventManager();

    protected abstract Task PerformInitialization();

    public async Task Initialize()
    {
        try
        {
            await _semaphore.WaitAsync();

            if (_isInitialized)
            {
                return;
            }
            await PerformInitialization();
            _isInitialized = true;
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    public void Subscribe(Func<Task> action)
    {
        _stateChanged.Subscribe(async () => await action());
    }

    protected void EmitChange()
    {
        _stateChanged.Emit();
    }
}

public interface IObservableBehind : INotBehind, ISingletonService
{
    Task Initialize();
    void Subscribe(Func<Task> action);
}
