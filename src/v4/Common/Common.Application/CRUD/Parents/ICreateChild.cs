using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD.Parents;

public interface ICreateChild<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Create(int parentId, T child);
}
