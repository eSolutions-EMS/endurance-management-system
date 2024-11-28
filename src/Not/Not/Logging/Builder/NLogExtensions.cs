using Microsoft.Extensions.DependencyInjection;
using Not.Logging.Filesystem;
using Not.Logging.HTTP;
using Not.Startup;

namespace Not.Logging.Builder;

public static class NLogExtensions
{
    public static NLogBuilder AddHttpLogger(this NLogBuilder builder, Action<HttpLoggerContext> configure)
    {
        var factory = ConfigureContext(configure);
        builder
            .Services.AddSingleton<IHttpLoggerConfiguration, HttpLoggerContext>(factory)
            .AddSingleton<IStartupInitializer, HttpLoggerInitializer>();

        return builder;
    }

    public static NLogBuilder AddFilesystemLogger(this NLogBuilder builder, Action<FilesystemLoggerContext> configure)
    {
        var factory = ConfigureContext(configure);
        builder
            .Services.AddSingleton<IFilesystemLoggerConfiguration, FilesystemLoggerContext>(factory)
            .AddSingleton<IStartupInitializer, FilesystemLoggerInitalizer>();

        return builder;
    }

    static Func<IServiceProvider, HttpLoggerContext> ConfigureContext(Action<HttpLoggerContext> configure)
    {
        var contex = new HttpLoggerContext();
        configure(contex);
        contex.Validate();
        return _ => contex;
    }

    static Func<IServiceProvider, FilesystemLoggerContext> ConfigureContext(Action<FilesystemLoggerContext> configure)
    {
        var contex = new FilesystemLoggerContext();
        configure(contex);
        contex.Validate();
        return _ => contex;
    }
}
