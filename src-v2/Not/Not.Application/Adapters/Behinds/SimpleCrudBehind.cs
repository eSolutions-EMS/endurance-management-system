using Not.Application.Contexts;
using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Domain;
using Not.Exceptions;
using Not.Reflection;
using Not.Safe;
using Not.Structures;

namespace Not.Application.Adapters.Behinds;

public abstract class SimpleCrudBehind<T, TModel> : ObservableBehind, IListBehind<T>, IFormBehind<TModel>, IDisposable
    where T : DomainEntity
{
    readonly Guid? _loadedSubscriptionId;
    readonly IParentContext<T>? _parentContext;
    readonly IRepository<T> _repository;

    protected SimpleCrudBehind(IRepository<T> repository, IParentContext<T> parentContext)
    {
        _repository = repository;
        _parentContext = parentContext;
        ObservableCollection = parentContext.Children;
        ObservableCollection.Sub(EmitChange);
        //_loadedSubscriptionId = _parentContext.Loaded.Subscribe(EmitChange);
    }
    protected SimpleCrudBehind(IRepository<T> repository)
    {
        _repository = repository;
        ObservableCollection = new(EmitChange);
    }

    protected EntitySet<T> ObservableCollection { get; }

    public IReadOnlyList<T> Items => ObservableCollection;

    protected abstract T CreateEntity(TModel model);
    protected abstract T UpdateEntity(TModel model);

    protected virtual async Task OnBeforeCreate(T entity)
    {
        if (_parentContext != null)
        {
            _parentContext.Add(entity);
            await _parentContext.Update();
        }
    }

    protected virtual async Task OnBeforeUpdate(T entity)
    {
        if (_parentContext != null)
        {
            _parentContext.Update(entity);
            await _parentContext.Update();
        }
    }

    protected virtual async Task OnBeforeDelete(T entity)
    {
        if (_parentContext != null)
        {
            _parentContext.Remove(entity);
            await _parentContext.Update();
        }
    }

    // TODO: probably remove SafeCreate and SafeUpdate as they interfere with Form validation mechanism
    // where errors are handled anyway. Use SafeHelper for form validation mechanism:
    // use optional parameter to override the default validation behavior
    async Task<TModel> SafeCreate(TModel model)
    {
        var entity = CreateEntity(model);
        await OnBeforeCreate(entity);
        await _repository.Create(entity);
        ObservableCollection.AddOrReplace(entity);
        return model;
    }

    async Task<TModel> SafeUpdate(TModel model)
    {
        var entity = UpdateEntity(model);
        await OnBeforeUpdate(entity);
        await _repository.Update(entity);
        ObservableCollection.AddOrReplace(entity);
        return model;
    }

    async Task<T> SafeDelete(T entity)
    {
        await OnBeforeDelete(entity);
        await _repository.Delete(entity);
        ObservableCollection.Remove(entity);
        return entity;
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
                throw GuardHelper.Exception(
                    $"{name} is used as standalone child behind. " +
                    $"I.e. it depends on parent context '{_parentContext.GetTypeName()}' which isn't loaded." +
                    $"Either use initialize the context preemptively or pass parentId to '{name}.{nameof(IObservableBehind.Initialize)}'");
            }
            if (arguments.Any())
            {
                var argument = arguments.First();
                if (argument is not int parentId)
                {
                    throw GuardHelper.Exception($"Invalid argument '{argument.GetTypeName()}'");
                }
                await _parentContext.Load(parentId);
            }
            //ObservableCollection.AddRange(_parentContext.GetChildren());
            return false; // Has to be false in order to be able to reintialize and update if any children are changed
        }
        else
        {
            var entities = await _repository.ReadAll();
            ObservableCollection.AddRange(entities);
            return entities.Any();
        }
    }

    public async Task<TModel> Create(TModel model)
    {
        return await SafeHelper.Run(() => SafeCreate(model)) ?? model;
    }

    public async Task<T> Delete(T entity)
    {
        return await SafeHelper.Run(() => SafeDelete(entity)) ?? entity;
    }

    public async Task<TModel> Update(TModel model)
    {
        return await SafeHelper.Run(() => SafeUpdate(model)) ?? model;
    }

    public void Dispose()
    {
        if (_loadedSubscriptionId != null)
        {
            _parentContext?.Loaded.Unsubscribe(_loadedSubscriptionId.Value); //TODO: test
        }
    }
}
