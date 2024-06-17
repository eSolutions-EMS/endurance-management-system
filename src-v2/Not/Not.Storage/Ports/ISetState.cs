using Not.Domain;

namespace Not.Storage.Ports.States;

public interface ISetState<T>
    where T : DomainEntity
{
    List<T> EntitySet { get; }
}