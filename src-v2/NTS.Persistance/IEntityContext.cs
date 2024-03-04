using Not.Domain;

namespace NTS.Persistence;

public interface IEntityContext<T>
    where T : DomainEntity
{
    List<T> Entities { get; }
}
