using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Setup;

public class SetupState : IRootStore<Event>
{
    public Event? Root { get; set; }
}