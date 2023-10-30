using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD;

public interface IParent<T> : IAdd<T>, IRemove<T>, ISingletonService
    where T : DomainEntity
{
}