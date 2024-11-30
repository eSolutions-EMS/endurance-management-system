using Microsoft.Extensions.DependencyInjection;
using Not.Contexts;
using Not.Logging.Builder;
using Not.Startup;
using Serilog;

namespace Not.Logging.Filesystem;

public class FilesystemLoggerInitalizer : IStartupInitializer
{
    readonly IFileContext _context;

    public FilesystemLoggerInitalizer([FromKeyedServices(NLogBuilder.KEY)] IFileContext context)
    {
        _context = context;
    }

    public void RunAtStartup()
    {
        LoggingHelper.Validate();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File($"{_context.Path}/log.txt")
            .CreateLogger();
    }
}
