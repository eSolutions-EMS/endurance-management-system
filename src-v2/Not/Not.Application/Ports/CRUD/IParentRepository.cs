using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface IParentRepository<T> : ICreateChild<T>, IUpdateChild<T>, IDeleteChild<T>
    where T : DomainEntity { }
