using Not.Events;
using Not.Injection;
using Not.Safe;

namespace Not.Blazor.Ports.Behinds;

// TODO: Probably rename to EventBehind. However we need to rename the Domain entity Event
// To EnduranceEvent to avoid name conflicts
public abstract class ObservableBehind : IObservableBehind
{
    readonly SemaphoreSlim _semaphore = new(1);
    bool _isInitialized;
    private readonly IEventManager _stateChanged = new EventManager();

    /// <summary>
    /// Initialize the state of an ObservableBehind. 
    /// If the state has been initialized successfully It cannot be initialized again.
    /// </summary>
    /// <returns>Indicates weather or not the state has been initialized successfully</returns>
    protected abstract Task<bool> PerformInitialization();

    public async Task Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        try
        {
            await _semaphore.WaitAsync();
            _isInitialized = await SafeHelper.Run(PerformInitialization);
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    public void Subscribe(Func<Task> action)
    {
        _stateChanged.Subscribe(action);
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
