using Core.Events;

namespace Core.Models;

public class Observable
{
    public void RaiseStateChanged(object value)
    {
        AppState.RaiseChanged(null, value);
    }

    public void RaiseStateChanged(object sender, object value)
    {
        AppState.RaiseChanged(sender, value);
    }
}
