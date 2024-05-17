using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface ISetRepository<T> : ICreate<T>, IReadAll<T>, IUpdate<T>, IDelete<T>
    where T : DomainEntity
{
}
