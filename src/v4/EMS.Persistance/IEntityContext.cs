using Common.Domain;

namespace EMS.Persistence;

public interface IEntityContext<T>
    where T : DomainEntity
{
    List<T> Entities { get; }
}
