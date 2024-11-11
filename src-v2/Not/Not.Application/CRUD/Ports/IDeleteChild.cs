using Not.Domain;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface IDeleteChild<T> : ITransient
    where T : DomainEntity
{
    Task<T> Delete(int parentId, T child);
}
