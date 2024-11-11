using Not.Domain;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface IReadAll<T> : ITransient
    where T : DomainEntity
{
    Task<IEnumerable<T>> ReadAll();
}
