using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD;

public interface IRead<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Read(int id);
}
