using Not.Domain;
using Not.Injection;

namespace Not.Application.Ports.CRUD;

public interface IUpdateChild<T> : ITransient
    where T : DomainEntity
{
    Task<T> Update(int parentId, T child);
}
