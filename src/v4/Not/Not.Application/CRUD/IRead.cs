using Not.Conventions;
using Not.Domain;

namespace Not.Application.CRUD;

public interface IRead<T> : ITransientService
    where T : DomainEntity
{
    Task<T?> Read(int id);
}
