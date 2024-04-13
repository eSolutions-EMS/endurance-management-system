using Not.Domain;
using NTS.Domain.Setup.Entities;

namespace NTS.Persistence;

public class State :
    IEntityContext<Event>,
    IEntityContext<Competition>
{
    private List<Event> _events = new();

    private List<Competition> _competitions = new();
    public Event? Event
    {
        get => _events.FirstOrDefault();
        set => _events = value == null ? new() : new() { value };
    }

    public List<Tandem> Tandems { get; } = new();

    public Competition? Competition
    {
        get => _competitions.FirstOrDefault();
        set => _competitions = value == null ? new() : new() { value };
    }
    public List<Official> Officials => Event?.Officials.ToList() ?? new();
    public List<Competition> Competitions => Event?.Competitions.ToList() ?? new();

    public List<Contestant> Contestants => Competition?.Contestants.ToList() ?? new();

    List<Event> IEntityContext<Event>.Entities => _events;
    List<Competition> IEntityContext<Competition>.Entities => Event?.Competitions.ToList() ?? new();
}