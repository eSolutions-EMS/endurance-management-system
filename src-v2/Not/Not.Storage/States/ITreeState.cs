using Not.Domain.Base;

namespace Not.Storage.States;

public interface ITreeState<T> : IState
    where T : DomainEntity
{
    T? Root { get; set; }
}
