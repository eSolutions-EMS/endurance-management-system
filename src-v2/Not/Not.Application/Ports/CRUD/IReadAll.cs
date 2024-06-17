using Not.Injection;
using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface IReadAll<T> : ITransientService
    where T : DomainEntity
{
    Task<IEnumerable<T>> ReadAll();
}
