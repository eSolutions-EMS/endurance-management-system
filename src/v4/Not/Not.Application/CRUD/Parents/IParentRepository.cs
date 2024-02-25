using Not.Domain;

namespace Not.Application.CRUD.Parents;

public interface IParentRepository<T> : ICreateChild<T>, IUpdateChild<T>, IDeleteChild<T>
    where T : DomainEntity
{
}
