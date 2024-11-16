using Not.Domain.Base;

namespace Not.Storage.States;

public interface ISetState<T> : IState
    where T : DomainEntity
{
    List<T> EntitySet { get; }
}
