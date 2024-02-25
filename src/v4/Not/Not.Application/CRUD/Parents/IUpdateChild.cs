using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD.Parents;

public interface IUpdateChild<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Update(T child);
}
