using Microsoft.Extensions.DependencyInjection;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.Static
{
    public static class ServiceProvider
    {
        private static IServiceProvider _provider;

        public static void Set(IServiceProvider provider)
        {
            _provider = provider;
        }

        public static void Initialize(IServiceProvider provider)
        {
            if (_provider != null)
            {
                throw new InvalidOperationException("ServiceProvider is already initialized");
            }
            _provider = provider;

        }

        public static T GetService<T>()
        {
            return _provider.GetService<T>();
        }
    }
}
