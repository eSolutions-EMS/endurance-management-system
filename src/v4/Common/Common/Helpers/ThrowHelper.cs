using Common.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Common.Helpers;

public static class ThrowHelper
{
    // TODO: Use CallerMemberName and similar to improve default message transparency
	[DoesNotReturn]
	public static void ThrowIfNull(object? value, string message = "Object cannot be null")
	{
		if (value == null)
		{
            throw new WhopsException(message);
        }
    }

    public static Exception Exception(string message)
    {
        return new WhopsException(message);
    }
}
