using Not.Structures;

namespace Not.Application.Adapters.Behinds;

public abstract class ObservableListBehind<T> : ObservableBehind, IDisposable
    where T : IIdentifiable
{
    readonly Guid _loadedSubscriptionId;

    protected ObservableListBehind()
    {
        _loadedSubscriptionId = ObservableList.ChangedEvent.Subscribe(EmitChange);
    }

    protected ObservableListBehind(ObservableList<T> list)
    {
        _loadedSubscriptionId = list.ChangedEvent.Subscribe(EmitChange);
        ObservableList = list;
    }

    protected ObservableList<T> ObservableList { get; } = [];

    public void Dispose()
    {
        ObservableList.ChangedEvent.Unsubscribe(_loadedSubscriptionId);
        GC.SuppressFinalize(this);
    }
}
