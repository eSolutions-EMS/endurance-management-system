using Microsoft.Extensions.DependencyInjection;
using Not.Contexts;
using Not.Storage.Stores.Config;

namespace NTS.Storage.Injection;

public static class StorageInjection
{
    // Necessary to be called directly from UI project, otherwise the runtime treeshakes this
    // DLL off, because no resources are explicitly referenced.
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        return services
            .AddJsonFileStore(x => x.Path = ContextHelper.GetAppDirectory("data"))
            .AddStaticOptionsStore(x => x.Path = ContextHelper.GetAppDirectory("resources"));
    }
}
