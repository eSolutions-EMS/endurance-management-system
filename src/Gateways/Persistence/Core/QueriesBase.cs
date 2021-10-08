using EnduranceJudge.Application.Contracts.Queries;
using EnduranceJudge.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Gateways.Persistence.Core
{
    public abstract class QueriesBase<T> : IQueries<T>
        where T : IDomainObject
    {
        protected QueriesBase(IDataContext context)
        {
            this.Context = context;
        }

        protected IDataContext Context { get; }

        protected abstract List<T> Set { get; }

        public T GetOne(Predicate<T> predicate) => this.Set.Find(predicate);
        public virtual T GetOne(int id) => this.Set.Find(x => x.Id == id);
        public virtual List<T> GetAll() => this.Set.ToList();
    }
}
