namespace Not.Exceptions;

/// <summary>
/// Use as base for domain exceptions (validation). That way functionalities like <seealso cref="Notifier.Notify"/>
/// can work with Domain exceptions which are higher up on the dependency chain and cannot be referenced directly
/// </summary>
public abstract class DomainExceptionBase(string message) : ApplicationException(message)
{
}
