using Not.Application.CRUD.Ports;
using Not.Blazor.CRUD.Forms.Ports;
using Not.Blazor.CRUD.Lists.Ports;
using Not.Blazor.Ports;
using Not.Domain;
using Not.Exceptions;
using Not.Reflection;
using Not.Safe;

namespace Not.Application.Behinds.Adapters;

public abstract class CrudBehind<T, TModel>
    : ObservableListBehind<T>,
        IListBehind<T>,
        IFormBehind<TModel>
    where T : DomainEntity
{
    readonly IParentContext<T>? _parentContext;
    readonly IRepository<T> _repository;

    /// <summary>
    /// Instantiates a CRUD behind capable of handling child items using <seealso cref="IParentContext{T}"/>
    /// </summary>
    /// <param name="repository">Items repository</param>
    /// <param name="parentContext">ParentContext defines the necessary operations in order to update item's parent when item changes</param>
    protected CrudBehind(IRepository<T> repository, IParentContext<T> parentContext)
        : base(parentContext.Children)
    {
        _repository = repository;
        _parentContext = parentContext;
    }

    /// <summary>
    /// Instatiates a basic CRUD behind for a standalone or root-level entity
    /// </summary>
    /// <param name="repository">Entity's repository</param
    protected CrudBehind(IRepository<T> repository)
        : base([])
    {
        _repository = repository;
    }

    protected abstract T CreateEntity(TModel model);
    protected abstract T UpdateEntity(TModel model);

    public IReadOnlyList<T> Items => ObservableList;

    protected virtual async Task OnBeforeCreate(T entity)
    {
        if (_parentContext != null)
        {
            _parentContext.Add(entity);
            await _parentContext.Persist();
        }
    }

    protected virtual async Task OnBeforeUpdate(T entity)
    {
        if (_parentContext != null)
        {
            _parentContext.Update(entity);
            await _parentContext.Persist();
        }
    }

    protected virtual async Task OnBeforeDelete(T entity)
    {
        if (_parentContext != null)
        {
            _parentContext.Remove(entity);
            await _parentContext.Persist();
        }
    }

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        if (_parentContext != null)
        {
            // CompetitionBehind assumes it's invoked in a EnduranceEvent tree context. I.e. that EnduranceEvent is already
            // initialized and _parentContext.Entity is not null. If it can be invoked autonomously then it can be initialized using args
            if (!_parentContext.HasLoaded() && !arguments.Any())
            {
                var name = this.GetTypeName();
                var message =
                    $"{name} is used as standalone child behind. "
                    + $"I.e. it depends on parent context '{_parentContext.GetTypeName()}' which isn't loaded."
                    + $"Either use initialize the context preemptively or pass parentId to '{name}.{nameof(IObservableBehind.Initialize)}'";
                throw GuardHelper.Exception(message);
            }
            if (arguments.Any())
            {
                var argument = arguments.First();
                if (argument is not int parentId)
                {
                    var message = $"Invalid argument '{argument.GetTypeName()}'";
                    throw GuardHelper.Exception(message);
                }
                await _parentContext.Load(parentId);
            }
            //ObservableCollection.AddRange(_parentContext.GetChildren());
            return false; // Has to be false in order to be able to reintialize and update if any children are changed
        }
        else
        {
            var entities = await _repository.ReadAll();
            ObservableList.AddRange(entities);
            return entities.Any();
        }
    }

    public async Task Update(TModel model)
    {
        var entity = UpdateEntity(model);
        await OnBeforeUpdate(entity);
        await _repository.Update(entity);
        ObservableList.AddOrReplace(entity);
    }

    public async Task Create(TModel model)
    {
        var entity = CreateEntity(model);
        await OnBeforeCreate(entity);
        await _repository.Create(entity);
        ObservableList.AddOrReplace(entity);
    }

    #region Safe pattern

    public async Task Delete(T entity)
    {
        await SafeHelper.Run(() => SafeDelete(entity));
    }

    async Task SafeDelete(T entity)
    {
        await OnBeforeDelete(entity);
        await _repository.Delete(entity);
        ObservableList.Remove(entity);
    }

    #endregion
}
