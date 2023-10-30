using System.Diagnostics.CodeAnalysis;

namespace Common.Helpers;

public static class ThrowHelper
{
	[DoesNotReturn]
	public static void ThrowIfNull(object? value, string message)
	{
		if (value == null)
		{
            throw new Exception(message);
        }
    }
}
