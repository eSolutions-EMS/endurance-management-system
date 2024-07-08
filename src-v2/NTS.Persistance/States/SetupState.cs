using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Setup;

public class SetupState : NotState, ITreeState<Event>, ISetState<Loop>, ISetState<Horse>, ISetState<Athlete>
{
    public Event? Event { get; set; }
    public List<Loop> Loops { get; } = new List<Loop>();
    public List<Horse> Horses { get; } = new List<Horse>();
    public List<Athlete> Athletes { get; } = new List<Athlete>();

    Event? ITreeState<Event>.Root
    {
        get => Event; 
        set => Event = value;
    }

    List<Loop> ISetState<Loop>.EntitySet => Loops;
    List<Horse> ISetState<Horse>.EntitySet => Horses;
    List<Athlete> ISetState<Athlete>.EntitySet => Athletes;
}