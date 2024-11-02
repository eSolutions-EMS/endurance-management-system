using System;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Utilities;

public static class StaticProvider
{
    private static IServiceProvider? provider;

    public static void Initialize(IServiceProvider provider)
    {
        if (StaticProvider.provider != null)
        {
            throw new InvalidOperationException("StaticProvider is already initialized");
        }
        StaticProvider.provider = provider;
    }

    public static T GetService<T>()
        where T : notnull
    {
        if (StaticProvider.provider == null)
        {
            throw new InvalidOperationException("StaticProvider is not initialized");
        }

        return provider.GetRequiredService<T>();
    }
}
