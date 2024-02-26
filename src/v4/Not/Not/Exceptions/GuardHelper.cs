using System.Diagnostics.CodeAnalysis;

namespace Not.Exceptions;

public static class GuardHelper
{
    // TODO: Use CallerMemberName and similar to improve default message transparency
    [DoesNotReturn]
    public static void ThrowIfNull(object? value, string message = "Object cannot be null")
    {
        if (value == null)
        {
            throw new GuardException(message);
        }
    }

    public static Exception Exception(string message)
    {
        return new GuardException(message);
    }
}
