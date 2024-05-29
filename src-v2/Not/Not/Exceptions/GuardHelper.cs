using Not.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Not.Exceptions;

/// <summary>
/// GuardHelper deals with exceptions that should never be seen by the end-user
/// </summary>
public static class GuardHelper
{
    /// <summary>
    /// Mainly used in order to prevent nullable warnings and guard against default values  
    /// </summary>
    /// <exception cref="GuardException"></exception>
    [DoesNotReturn]
    public static void ThrowIfDefault<T>(T value)
    {
        if (value?.Equals(default(T)) ?? true)
        {
            throw new GuardException($"{ReflectionHelper.GetName<T>()} cannot be default");
        }
    }

    public static Exception Exception(string message)
    {
        return new GuardException(message);
    }
}
