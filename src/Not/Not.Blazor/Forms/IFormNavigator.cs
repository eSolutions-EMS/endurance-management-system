using Not.Injection;
using Not.Domain;
using Not.Blazor.TM.Forms.Components;

namespace Not.Blazor.Forms;

/// <summary>
/// Abstraction that facilitates Form Navigation
/// </summary>
/// <typeparam name="T">Type of Domain Entity</typeparam>
/// <typeparam name="TFields">
/// Blazor component implementing <seealso cref="IFormFields{T}"/> 
/// and wraps form input field components for entity <typeparamref name="T"/>
/// </typeparam>
public interface IFormNavigator<T, TFields> : ITransientService
    where T : DomainEntity
    where TFields : NotForm<T>
{
    /// <summary>
    /// Renders a dialog using <typeparamref name="TFields"/> for entity <typeparamref name="T"/>
    /// </summary>
    /// <returns></returns>
    Task Create();
    /// <summary>
    /// Navigates a Blazor component that renders <typeparamref name="TFields"/> for entity <typeparamref name="T"/>
    /// </summary>
    /// <param name="endpoint">The route to the update component</param>
    /// <param name="entity">The existing entity instance</param>
    /// <returns></returns>
    Task Update(string endpoint, T entity);
}
