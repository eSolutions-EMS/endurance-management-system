using Microsoft.Extensions.DependencyInjection;

namespace Not.Logging.Builder;

public class NLogBuilder
{
    public NLogBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}
