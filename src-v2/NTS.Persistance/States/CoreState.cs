using Not.Storage.Ports.States;
using NTS.Domain.Core.Entities;

namespace NTS.Persistence.States;

public class CoreState : IFlatState<Event>, ISetState<Official>
{
    public Event? Event { get; set; }
    public List<Official> Officials { get; } = new();

    Event? IFlatState<Event>.Entity
    {
        get => Event;
        set => Event = value;
    }
    List<Official> ISetState<Official>.EntitySet => Officials;
}
