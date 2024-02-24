using Common.Domain;

namespace Not.Blazor.Forms;

/// <summary>
/// Abstraction representing a reusable wrapper for input field components for <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">Type of Domain Entity</typeparam>
public interface IFormFields<T>
    where T : DomainEntity
{
    /// <summary>
    /// Construct an instance of <typeparamref name="T"/> based on the form inputs that is suitable for create.
    /// I.e. without pre-existing Identifier
    /// </summary>
    /// <returns><typeparamref name="T"/> instance for a new Domain Entity</returns>
    T SubmitCreate();
    /// <summary>
    /// Construct an instance of <typeparamref name="T"/> based on the form inputs that is suitable for update.
    /// I.e. with pre-existing Identifier
    /// </summary>
    /// <returns><typeparamref name="T"/> instance for existing Domain Entity to be updated</returns>
    T SubmitUpdate();
    /// <summary>
    /// Adds validation error for a specific input field, or generic error if <paramref name="field"/> is null
    /// </summary>
    /// <param name="field">Name of the input field. The "field - name" mapping is a responsibility of the implementations</param>
    /// <param name="message">Validation message</param>
    /// <returns></returns>
    Task AddValidationError(string? field, string message);
}
