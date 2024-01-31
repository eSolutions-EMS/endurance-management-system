using Common.Domain;

namespace Not.Blazor.Dialogs;

public interface INotCreateDialog<T, TModel>
    where T : DomainEntity
    where TModel : new()
{
    Func<TModel, T> Factory { get; }
}
