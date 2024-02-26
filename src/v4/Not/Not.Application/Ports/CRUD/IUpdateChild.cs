using Not.Conventions;
using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface IUpdateChild<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Update(T child);
}
