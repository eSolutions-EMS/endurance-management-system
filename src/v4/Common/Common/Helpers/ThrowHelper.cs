using System.Diagnostics.CodeAnalysis;

namespace Common.Helpers;

public static class ThrowHelper
{
	[DoesNotReturn]
	public static void ThrowIfNull(object? value, string message)
	{
		throw new Exception(message);
	}
}
