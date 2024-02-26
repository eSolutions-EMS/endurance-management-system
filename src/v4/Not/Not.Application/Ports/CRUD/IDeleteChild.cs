using Not.Conventions;
using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface IDeleteChild<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Delete(int parentId, T child);
}
