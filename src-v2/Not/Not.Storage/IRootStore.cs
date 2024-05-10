using Not.Domain;

namespace Not.Storage;

public interface IRootStore<T>
    where T : DomainEntity
{
    T? Root { get; set; }
}
