using Microsoft.Extensions.DependencyInjection;

namespace NTS.Storage.Injection;

public static class StorageInjection
{
    // Necessary to be called directly from UI project, otherwise the runtime treeshakes this
    // DLL off, because no resources are explicitly referenced.
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        return services
            .AddJsonFileStore()
            .AddStaticOptionsStore();
    }
}
