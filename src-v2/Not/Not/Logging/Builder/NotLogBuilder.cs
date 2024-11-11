using Microsoft.Extensions.DependencyInjection;

namespace Not.Logging.Builder;

public class NotLogBuilder
{
    public NotLogBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}
