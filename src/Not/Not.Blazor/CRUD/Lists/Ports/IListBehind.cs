using Not.Blazor.CRUD.Ports;
using Not.Blazor.Ports;

namespace Not.Blazor.CRUD.Lists.Ports;

public interface IListBehind<T> : IDeleteBehind<T>, IObservableBehind
{
    IReadOnlyList<T> Items { get; }
}
