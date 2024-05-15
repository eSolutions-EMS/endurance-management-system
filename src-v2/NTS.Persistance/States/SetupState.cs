using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Setup;

public class SetupState : IRootStore<Event>, IRootStore<Phase>
{
    public Event? Event { get; set; }
    public Phase? Phase { get; set; }
    
    Event? IRootStore<Event>.Root
    {
        get => Event; 
        set => Event = value;
    }

    Phase? IRootStore<Phase>.Root
    {
        get => Phase;
        set => Phase = value;
    }
}