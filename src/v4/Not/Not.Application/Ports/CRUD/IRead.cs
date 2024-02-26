using Not.Conventions;
using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface IRead<T> : ITransientService
    where T : DomainEntity
{
    Task<T?> Read(int id);
}
