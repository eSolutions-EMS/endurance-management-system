using Not.Domain;

namespace Not.Application.CRUD.Ports;

public interface IParentRepository<T> : ICreateChild<T>, IUpdateChild<T>, IDeleteChild<T>
    where T : DomainEntity
{ }
