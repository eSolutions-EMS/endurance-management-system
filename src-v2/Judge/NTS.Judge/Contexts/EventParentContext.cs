using Not.Application.Contexts;
using Not.Application.Ports.CRUD;
using Not.Structures;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Contexts;

public class EventParentContext(IRepository<Event> entities) : BehindContext<Event>(entities),
    IParentContext<Competition>,
    IParentContext<Official>
{
    EntitySet<Competition> _competitions = new();
    EntitySet<Official> _officials = new();

    EntitySet<Competition> IParentContext<Competition>.Children
    {
        get => _competitions;
        set => _competitions = value;
    }
    EntitySet<Official> IParentContext<Official>.Children 
    {
        get => _officials;
        set => _officials = value; 
    }

    public async Task Load(int parentId)
    {
        Entity = await Repository.Read(parentId);
        if (_competitions != null)
        {
            _competitions.AddRange(Entity!.Competitions);
        }
        if (_officials != null)
        {
            _officials.AddRange(Entity!.Officials);
        }
    }

    public void Add(Competition child)
    {
        Entity!.Add(child);
    }
    public void Update(Competition child)
    {
        Entity!.Update(child);
    }
    public void Remove(Competition child)
    {
        Entity!.Remove(child);
    }

    public void Add(Official child)
    {
        Entity?.Add(child);
    }
    public void Update(Official child)
    {
        Entity!.Update(child);
    }
    public void Remove(Official child)
    {
        Entity!.Remove(child);
    }
}
