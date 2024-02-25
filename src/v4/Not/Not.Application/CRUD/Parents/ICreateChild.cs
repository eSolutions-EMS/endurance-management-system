using Not.Conventions;
using Not.Domain;

namespace Not.Application.CRUD.Parents;

public interface ICreateChild<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Create(int parentId, T child);
}
