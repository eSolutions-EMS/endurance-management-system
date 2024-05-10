using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Setup;

public class SetupState : IRootStore<Event>
{
    public Event? Event { get; set; }
    
    Event? IRootStore<Event>.Root
    {
        get => Event; 
        set => Event = value;
    }
}