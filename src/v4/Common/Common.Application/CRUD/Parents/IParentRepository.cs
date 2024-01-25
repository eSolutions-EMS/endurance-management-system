using Common.Domain;

namespace Common.Application.CRUD.Parents;

public interface IParentRepository<T> : ICreateChild<T>, IUpdateChild<T>, IDeleteChild<T>
    where T : DomainEntity
{
}
