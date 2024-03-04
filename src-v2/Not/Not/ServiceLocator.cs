using Microsoft.Extensions.DependencyInjection;
using Not.Exceptions;

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
		GuardHelper.ThrowIfNull(_provider, "ServiceLocator is not Initialized");

		return _provider.GetRequiredService<T>();
	}
}
