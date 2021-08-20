using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Core
{
    public class RepositoryBase<TDataStore, TEntityModel, TDomainModel> : ICommandsBase<TDomainModel>
        where TDomainModel : class, IDomainModel
        where TDataStore : IDataStore
        where TEntityModel : EntityBase
    {
        private readonly IWorkFileUpdater workFileUpdater;

        public RepositoryBase(TDataStore dataStore, IWorkFileUpdater workFileUpdater)
        {
            this.workFileUpdater = workFileUpdater;
            this.DataStore = dataStore;
        }

        protected TDataStore DataStore { get; }

        public virtual async Task<TDomainModel> Find(int id)
        {
            var item = await this.Find<TDomainModel>(id);
            return item;
        }

        public virtual async Task<TModel> Find<TModel>(int id)
        {
            var model = await this.DataStore
                .Set<TEntityModel>()
                .Where(x => x.Id == id)
                .MapQueryable<TModel>()
                .FirstOrDefaultAsync();
            return model;
        }

        public virtual async Task<IList<TModel>> All<TModel>()
        {
            var list = await this.DataStore
                .Set<TEntityModel>()
                .MapQueryable<TModel>()
                .ToListAsync();
            return list;
        }

        public async Task Save(TDomainModel domainModel, CancellationToken cancellationToken)
        {
            await this.InnerSave(domainModel, cancellationToken);
        }

        public async Task<T> Save<T>(TDomainModel domainModel, CancellationToken cancellationToken)
        {
            var entity = await this.InnerSave(domainModel, cancellationToken);
            return entity.Map<T>();
        }

        private async Task<TEntityModel> InnerSave(TDomainModel domain, CancellationToken cancellationToken)
        {
            var entity = await this.DataStore.FindAsync<TEntityModel>(domain.Id);
            if (entity == null)
            {
                entity = domain.Map<TEntityModel>();
                this.DataStore.Add(entity);
            }
            else
            {
                entity.MapFrom(domain);
                this.DataStore.Update(entity);
            }

            await this.DataStore.SaveChangesAsync(cancellationToken);
            await this.workFileUpdater.Snapshot();

            return entity;
        }
    }
}
