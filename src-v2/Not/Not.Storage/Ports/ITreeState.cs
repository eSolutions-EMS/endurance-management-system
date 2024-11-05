using Not.Domain;

namespace Not.Storage.Ports.States;

public interface ITreeState<T>
    where T : DomainEntity
{
    T? Root { get; set; }
}
