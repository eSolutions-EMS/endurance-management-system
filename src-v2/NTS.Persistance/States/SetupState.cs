using Not.Storage.Ports.States;
using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Setup;

public class SetupState : ITreeState<Event>
{
    public Event? Event { get; set; }
    
    Event? ITreeState<Event>.Root
    {
        get => Event; 
        set => Event = value;
    }
}