using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Core
{
    public class Repository<TEntityModel, TDomainModel> : ICommands<TDomainModel>
        where TDomainModel : class, IDomainModel
        where TEntityModel : EntityBase
    {
        private readonly IWorkFileUpdater workFileUpdater;

        public Repository(EnduranceJudgeDbContext dbContext, IWorkFileUpdater workFileUpdater)
        {
            this.workFileUpdater = workFileUpdater;
            this.DbContext = dbContext;
        }

        protected EnduranceJudgeDbContext DbContext { get; }

        public virtual async Task<TDomainModel> Find(int id)
        {
            var item = await this.Find<TDomainModel>(id);
            return item;
        }

        public virtual async Task<TModel> Find<TModel>(int id)
        {
            var model = await this.DbContext
                .Set<TEntityModel>()
                .Where(x => x.Id == id)
                .MapQueryable<TModel>()
                .FirstOrDefaultAsync();
            return model;
        }

        public virtual async Task<IList<TModel>> All<TModel>()
        {
            var list = await this.DbContext
                .Set<TEntityModel>()
                .MapQueryable<TModel>()
                .ToListAsync();
            return list;
        }

        public async Task Save(TDomainModel domain, CancellationToken token)
        {
            await this.InnerSave(domain, token);
        }

        public async Task<T> Save<T>(TDomainModel domain, CancellationToken token)
        {
            var entity = await this.InnerSave(domain, token);
            return entity.Map<T>();
        }

        public async Task Remove(int id, CancellationToken token)
        {
            var entity = await this.DbContext
                .Set<TEntityModel>()
                .FirstOrDefaultAsync(x => x.Id == id, token);

            if (entity == null)
            {
                throw new InvalidOperationException("Cannot delete Entity with Id '{id}' is null.");
            }

            this.DbContext
                .Set<TEntityModel>()
                .Remove(entity);

            await this.Persist(token);
        }

        private async Task<TEntityModel> InnerSave(TDomainModel domain, CancellationToken token)
        {
            var entity = await this.DbContext.FindAsync<TEntityModel>(new object[] { domain.Id }, token);
            if (entity == null)
            {
                entity = domain.Map<TEntityModel>();
                this.DbContext.Add(entity);
            }
            else
            {
                entity.MapFrom(domain);
                this.DbContext.Update(entity);
            }

            await this.Persist(token);

            return entity;
        }

        protected async Task Persist(CancellationToken token)
        {
            await this.DbContext.SaveChangesAsync(token);
            await this.workFileUpdater.Snapshot();
        }
    }
}
