using System;

namespace Core.Events;

public static class AppState
{
	public static event EventHandler<object>? Changed;
	internal static void RaiseChanged(object? sender, object value)
	{
		Changed?.Invoke(sender, value);
	}
}
