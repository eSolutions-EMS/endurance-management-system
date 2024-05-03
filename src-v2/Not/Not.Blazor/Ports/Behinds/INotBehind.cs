using Not.Domain;

namespace Not.Blazor.Ports.Behinds;

/// <summary>
/// <seealso cref="INotBehind{T}"/> is a <seealso cref="INotBehind"/> that represents CRUD operations for a <seealso cref="DomainEntity"/>
/// <typeparamref name="T">Type of domain entity</typeparamref>
/// </summary>
public interface INotBehind<T> : ICreateBehind<T>, IReadBehind<T>, IUpdateBehind<T>, IDeleteBehind<T>, INotBehindWithChildren<T>
    where T : DomainEntity, IParent
{
}

/// <summary>
/// <seealso cref="INotBehind"/> is inspired from code-behind (i.e. the code that sits behind a view component)
/// </summary>
/// <remarks>
/// In Not.TM <seealso cref="INotBehind"/> serves as entry point of any application logic
/// </remarks>
public interface INotBehind
{
}