using Common.Conventions;
using Common.Domain;

namespace Common.Application.Behinds;

/// <summary>
/// <seealso cref="IParentBehind{T}"/> represents a <seealso cref="IBehind"/> that manages child <seealso cref="DomainEntity"/> objects
/// (see <seealso cref="IParent{T}"/> for more). Details regaring it's parent entity are responsiblity of the implementation
/// </summary>
/// <typeparam name="T">Type of the child entity</typeparam>
public interface IParentBehind<T> : ISingletonService
    where T : DomainEntity
{
    IEnumerable<T> Children { get; }
    Task<T> Create(T child);
    Task<T> Delete(T child);
    Task<T> Update(T child);
}
