using Common.Domain;
using Common.Domain.Ports;

namespace EMS.Persistence;

public abstract class RepositoryBase<T> : IRepository<T>
    where T : DomainEntity
{
    protected List<T> Data { get; } = new();

    public virtual Task<T> Create(T entity)
    {
        this.Data.Add(entity);
        return Task.FromResult(entity);
    }

    public virtual Task Delete(T entity)
    {
        this.Data.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task Delete(int id)
    {
        var entity = this.Data.FirstOrDefault(x => x.Id == id);
        if (entity != null)
        {
            this.Data.Remove(entity);
        }
        return Task.CompletedTask;
    }

    public virtual Task<int> Delete(Predicate<T> filter)
    {
        var count = this.Data.RemoveAll(filter);
        return Task.FromResult(count);
    }

    public virtual Task<T?> Read(int id)
    {
        var entity = this.Data.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(entity);
    }

    public virtual Task<IEnumerable<T>> Read(Predicate<T> filter)
    {
        var entities = this.Data.Where(x => filter(x));
        return Task.FromResult(entities);
    }

    public Task<T> Update(T entity)
    {
        var match = this.Data.FirstOrDefault(x => x == entity);
        if (match != null)
        {
            this.Data.Remove(match);
        }
        this.Data.Add(entity);
        return Task.FromResult(entity);
    }
}
