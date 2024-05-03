using Not.Injection;
using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface IUpdateChild<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Update(int parentId, T child);
}
