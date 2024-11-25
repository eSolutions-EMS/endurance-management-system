using Not.Domain.Base;
using Not.Injection;

namespace Not.Application.CRUD.Ports;

public interface IRead<T> : ITransient
    where T : AggregateRoot
{
    Task<T?> Read(int id);
}
