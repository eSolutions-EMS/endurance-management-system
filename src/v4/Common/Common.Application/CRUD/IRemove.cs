using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD;

public interface IRemove<T>
    where T : DomainEntity
{
    Task Remove(T child);
}
