using Not.Domain;

namespace Not.Storage;

public interface IFlatSet<T>
    where T : DomainEntity
{
    List<T> EntitySet { get; set; }
}
