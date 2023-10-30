using Common.Domain;
using Common.Domain.Ports;
using Common.Exceptions;

namespace Common.Application.CRUD;

public abstract class CrudBase<T, TCreate, TUpdate> : ICreate<TCreate>, IRead<T>, IUpdate<TUpdate>, IDelete<T>
    where T : DomainEntity
    where TCreate : class, new()
    where TUpdate : class, IIdentifiable
{
    public CrudBase(IRepository<T> repository)
    {
        this.Repository = repository;
    }

    public TCreate CreateModel { get; private set; } = new();
    public TUpdate? UpdateModel { get; private set; }

    public async Task Create()
    {
        var domainModel = Build(CreateModel);
        await this.Repository.Create(domainModel);
        this.UpdateModel = BuildUpdateModel(domainModel);
        this.CreateModel = new();
    } 

    public async Task<T> Read(int id)
    {
        var entity = await this.Repository.Read(id)
            ?? throw new Exception($"Domain entity '{typeof(T).Name}' with Id '{id}' does not exist");
        this.UpdateModel = BuildUpdateModel(entity);
        return entity;
    }

    public async Task Update()
    {
        if (this.UpdateModel is null)
        {
            throw new Exception($"Cannot perform '{nameof(this.Update)}' {nameof(this.UpdateModel)} is null");
        } 
        var entity = Build(this.UpdateModel);
        await this.Repository.Update(entity);
    }

    public async Task Delete(T entity)
    {
        if (this.UpdateModel is null)
        {
            return;
        }
        await this.Repository.Delete(entity.Id);
        this.UpdateModel = null;
    }

    protected IRepository<T> Repository { get; }
    protected T Entity
    {
        get
        {
            if (this.UpdateModel == null)
            {
                throw new WhopsException($"'{nameof(Entity)}' cannot be constructed because '{nameof(UpdateModel)}' is null");
            }
            return this.Build(this.UpdateModel);
        }
    }

    protected abstract T Build(TCreate model);
    protected abstract T Build(TUpdate model);
    protected abstract TUpdate BuildUpdateModel(T model);
}
