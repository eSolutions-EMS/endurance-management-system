namespace NTS.Judge.MAUI.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddMauiServerServices(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}

public class JudgeServiceProvider : IJudgeServiceProvider
{
    readonly IServiceProvider _serviceProvider;

    public JudgeServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
}

public interface IJudgeServiceProvider : IServiceProvider { }
