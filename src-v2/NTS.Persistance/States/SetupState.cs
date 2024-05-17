using Not.Storage.Ports.States;
using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Setup;

public class SetupState : ITreeState<Event>, ISetState<Lap>
{
    public Event? Event { get; set; }
    public List<Lap> Laps { get; set; } 

    public SetupState()
    {
        Laps = new List<Lap> { };
    }

    Event? ITreeState<Event>.Root
    {
        get => Event; 
        set => Event = value;
    }
    List<Lap> ISetState<Lap>.EntitySet { 
        get => Laps;
    }
}