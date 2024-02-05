using EMS.Domain.Objects;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence;

public class State : IState
{
    internal Action? DetachAction { private get; set; }
    public Event? Event { get; set; } = new Event("test", new Country("BGN", "Bulgaria"));
    public List<Official> Officials => Event?.Officials.ToList() ?? new List<Official>();

    public void Dispose()
    {
        DetachAction?.Invoke();
    }
}

public interface IState : IDisposable
{
    Event? Event { get; set; }
    List<Official> Officials { get; }
}