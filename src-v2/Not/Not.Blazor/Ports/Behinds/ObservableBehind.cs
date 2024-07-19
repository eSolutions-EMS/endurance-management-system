using Not.Events;
using Not.Startup;

namespace Not.Blazor.Ports.Behinds;

// TODO: Probably rename to EventBehind. However we need to rename the Domain entity Event
// To EnduranceEvent to avoid name conflicts
public abstract class ObservableBehind : IObservableBehind
{
    private IEventManager _stateChanged = new EventManager();

    public void Subscribe(Func<Task> action)
    {
        _stateChanged.Subscribe(async () => await action());
    }

    public void EmitChange()
    {
        _stateChanged.Emit();
    }
}

public interface IObservableBehind : INotBehind
{
    void Subscribe(Func<Task> action);
}
