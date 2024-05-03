using Not.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Not.Exceptions;

/// <summary>
/// GuardHelper deals with exceptions that should never be seen by the end-user
/// </summary>
public static class GuardHelper
{
    /// <summary>
    /// Mainly used in order to prevent nullable warnings
    /// </summary>
    /// <exception cref="GuardException"></exception>
    [DoesNotReturn]
    public static void ThrowIfNull(object? value, string? message = null)
    {
        if (value == null)
        {
            throw new GuardException(message ?? $"Object cannot be null");
        }
    }
    
    public static void ThrowIfDefault<T>(T value, string? message = null)
    {
        if (value == null)
        {
            throw new GuardException(message ?? $"{ReflectionHelper.GetName<T>()} cannot be null");
        }
    }

    public static Exception Exception(string message)
    {
        return new GuardException(message);
    }
}
