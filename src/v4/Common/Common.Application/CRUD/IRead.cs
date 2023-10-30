using Common.Domain;

namespace Common.Application.CRUD;

public interface IRead<T>
    where T : DomainEntity
{
    Task<T> Read(int id);
}
