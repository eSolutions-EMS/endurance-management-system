using Not.Events;
using Not.Injection;

namespace Not.Blazor.Ports.Behinds;

// TODO: Probably rename to EventBehind. However we need to rename the Domain entity Event
// To EnduranceEvent to avoid name conflicts
public abstract class ObservableBehind : IObservableBehind
{
    private IEventManager _stateChanged = new EventManager();

    public abstract Task Initialize();
    
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
