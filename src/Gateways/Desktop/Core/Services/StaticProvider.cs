using Microsoft.Extensions.DependencyInjection;
using System;

namespace EnduranceJudge.Gateways.Desktop.Core.Services
{
    public static class StaticProvider
    {
        private static IServiceProvider _provider;

        public static void Set(IServiceProvider provider)
        {
            _provider = provider;
        }

        public static IServiceProvider Provider
        {
            set
            {
                _provider = value;
            }
        }

        public static T GetService<T>()
        {
            return _provider.GetService<T>();
        }
    }
}
