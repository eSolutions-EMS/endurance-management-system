using Common.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Common.Helpers;

public static class ThrowHelper
{
	[DoesNotReturn]
	public static void ThrowIfNull(object? value, string message = "Object cannot be null")
	{
		if (value == null)
		{
            throw new WhopsException(message);
        }
    }
}
