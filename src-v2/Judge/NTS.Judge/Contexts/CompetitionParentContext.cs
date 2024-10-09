﻿using Not.Application.Contexts;
using Not.Application.Ports.CRUD;
using Not.Structures;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Contexts;

public class CompetitionParentContext(IRepository<Competition> entities) : BehindContext<Competition>(entities),
    IParentContext<Phase>,
    IParentContext<Contestant>
{
    EntitySet<Phase> _phases = new();
    EntitySet<Contestant> _contestants = new();

    EntitySet<Phase> IParentContext<Phase>.Children
    {
        get => _phases;
        set => _phases = value;
    }
    EntitySet<Contestant> IParentContext<Contestant>.Children
    {
        get => _contestants;
        set => _contestants = value;
    }

    public async Task Load(int parentId)
    {
        Entity = await Repository.Read(parentId);
        if (_phases != null)
        {
            _phases.AddRange(Entity!.Phases);
        }
        if (_contestants != null)
        {
            _contestants.AddRange(Entity!.Contestants);
        }
    }

    public void Add(Phase child)
    {
        Entity!.Add(child);
    }
    public void Update(Phase child)
    {
        Entity!.Update(child);
    }
    public void Remove(Phase child)
    {
        Entity!.Remove(child);
    }

    public void Add(Contestant child)
    {
        Entity!.Add(child);
    }
    public void Update(Contestant child)
    {
        Entity!.Update(child);
    }
    public void Remove(Contestant child)
    {
        Entity!.Remove(child);
    }
}
