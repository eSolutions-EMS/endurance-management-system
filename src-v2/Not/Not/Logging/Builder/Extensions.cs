using Microsoft.Extensions.DependencyInjection;
using Not.Logging.Loggers.Filesystem;
using Not.Logging.Loggers.HTTP;
using Not.Startup;

namespace Not.Logging.Builder;

public static class Extensions
{
    public static NotLogBuilder AddHttpLogger<T>(this NotLogBuilder builder)
        where T : class, IHttpLoggerConfiguration, new()
    {
        builder
            .Services.AddSingleton<IHttpLoggerConfiguration, T>()
            .AddSingleton<IStartupInitializer, HttpLoggerInitializer>();

        return builder;
    }

    public static NotLogBuilder AddFilesystemLogger<T>(this NotLogBuilder builder)
        where T : class, IFilesystemLoggerConfiguration, new()
    {
        builder
            .Services.AddSingleton<IFilesystemLoggerConfiguration, T>()
            .AddSingleton<IStartupInitializer, FilesystemLoggerInitalizer>();

        return builder;
    }
}
