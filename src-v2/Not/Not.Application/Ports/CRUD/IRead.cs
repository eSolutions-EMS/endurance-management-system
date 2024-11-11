using Not.Domain;
using Not.Injection;

namespace Not.Application.Ports.CRUD;

public interface IRead<T> : ITransient
    where T : DomainEntity
{
    Task<T?> Read(int id);
}
