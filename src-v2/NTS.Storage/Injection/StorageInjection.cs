using Microsoft.Extensions.DependencyInjection;

namespace NTS.Storage.Injection;

public static class StorageInjection
{
    /// <summary>
    /// Necessary to be called directly from UI project, otherwise the runtime treeshakes this
    /// DLL off, because no resources are explicitly referenced.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        return services;
    }
}
