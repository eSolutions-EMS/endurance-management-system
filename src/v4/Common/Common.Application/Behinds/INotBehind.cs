using Common.Conventions;
using Common.Domain;

namespace Common.Application.Behinds;

/// <summary>
/// <seealso cref="INotBehind{T}"/> is a <seealso cref="INotBehind"/> that represents CRUD operations for a <seealso cref="DomainEntity"/>
/// <typeparamref name="T">Type of domain entity</typeparamref>
/// </summary>
public interface INotBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T?> Read(int id);
    Task<T> Create(T entity); 
    Task<T> Update(T entity);
    Task<T> Delete(int id);
}

/// <summary>
/// <seealso cref="INotBehind"/> is inspired from code-behind (i.e. the code that sits behind a view component)
/// </summary>
/// <remarks>
/// In Not.TM <seealso cref="INotBehind"/> serves as entry poing of any application logic
/// </remarks>
public interface INotBehind
{
}