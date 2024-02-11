using Common.Conventions;
using Common.Domain;

namespace Common.Application.Behinds;

/// <summary>
/// <seealso cref="IBehind{T}"/> is a <seealso cref="IBehind"/> that represents CRUD operations for a <seealso cref="DomainEntity"/>
/// <typeparamref name="T">Type of domain entity</typeparamref>
/// </summary>
public interface IBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T?> Read(int id);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(int id);
}

/// <summary>
/// <seealso cref="IBehind"/> is inspired from code-behind (i.e. the code that sits behind a view component)
/// </summary>
/// <remarks>
/// In Not.TM <seealso cref="IBehind"/> serves as entry poing of any application logic
/// </remarks>
public interface IBehind
{

}