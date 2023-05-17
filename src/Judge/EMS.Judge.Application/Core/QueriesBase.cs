using Core.Domain.Core.Models;
using Core.Domain.State;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Application.Core;

public abstract class QueriesBase<T> : IQueries<T>
    where T : IDomain
{
    protected QueriesBase(IStateContext context)
    {
        this.State = context.State;
    }

    protected IState State { get; }

    protected abstract List<T> Set { get; }

    public virtual T GetOne(Predicate<T> predicate) => this.Set.Find(predicate);
    public virtual T GetOne(int id) => this.Set.Find(x => x.Id == id);
    public virtual List<T> GetAll() => this.Set.ToList();
}

public interface IQueries<T>
    where T : IDomain
{
    T GetOne(int id);
    List<T> GetAll();
}
