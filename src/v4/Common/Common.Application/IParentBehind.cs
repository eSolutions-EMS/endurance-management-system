using Common.Conventions;
using Common.Domain;

namespace Common.Application;

public interface IParentBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task Init(int parentId);
    DomainEntity? Parent { get; }
    IEnumerable<T> Children { get; }
    Task<T> Create(T child);
    Task Delete(T child);
    Task<T> Update(T child);
}
