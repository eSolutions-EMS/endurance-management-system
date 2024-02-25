using Not.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Not;

public static class ServiceLocator
{
	private static IServiceProvider? _provider;
	 
	public static void Initialize(IServiceProvider provider)
	{
		_provider = provider;
	}

	public static T Get<T>()
		where T : class
	{
		ThrowHelper.ThrowIfNull(_provider, "ServiceLocator is not Initialized");

		return _provider.GetRequiredService<T>();
	}
}
