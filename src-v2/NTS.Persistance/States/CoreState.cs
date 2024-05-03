using NTS.Domain.Core.Entities;

namespace NTS.Persistence.States;

public class CoreState : IFlatState<Event>
{
    Event? IFlatState<Event>.Entity { get; set; }
}
