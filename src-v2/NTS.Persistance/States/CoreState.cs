using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Entities.ParticipationAggregate;

namespace NTS.Persistence.States;

public class CoreState : IFlatState<Event>, ISetState<Official>, ISetState<Participation>
{
    public Event? Event { get; set; }
    public List<Official> Officials { get; } = new();
    public List<Participation> Participations { get; } = new();

    Event? IFlatState<Event>.Entity
    {
        get => Event;
        set => Event = value;
    }
    List<Official> ISetState<Official>.EntitySet => Officials;
    List<Participation> ISetState<Participation>.EntitySet => Participations;
}
