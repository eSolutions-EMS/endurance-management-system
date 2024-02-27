using NTS.Domain.Setup.Entities;

namespace NTS.Persistence;

public class State :
    IEntityContext<Event>,
    IEntityContext<Competition>
{
    private List<Event> _events = new();
    public Event? Event
    {
        get => _events.FirstOrDefault();
        set => _events = value == null ? new() : new() { value };
    }
    public List<Official> Officials => Event?.Officials.ToList() ?? new();
    public List<Competition> Competitions => Event?.Competitions.ToList() ?? new();

    List<Event> IEntityContext<Event>.Entities => _events;
    List<Competition> IEntityContext<Competition>.Entities => Event?.Competitions.ToList() ?? new();
}