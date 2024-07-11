using EMS.Judge.Api.Services;
using Not.Injection;

namespace NTS.Judge.MAUI.Server;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddMauiServerServices(this WebApplicationBuilder builder, IServiceProvider callerProvider)
    {
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<IJudgeServiceProvider>(new JudgeServiceProvider(callerProvider));
        builder.Services.AddHostedService<NetworkBroadcastService>();
        builder.Services.GetConventionalAssemblies().RegisterConventionalServices();
        return builder;
    }
}

public class JudgeServiceProvider : IJudgeServiceProvider
{
    private readonly IServiceProvider _serviceProvider;

    public JudgeServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
}

public interface IJudgeServiceProvider : IServiceProvider
{
}
