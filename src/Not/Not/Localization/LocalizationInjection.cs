using Microsoft.Extensions.DependencyInjection;

namespace Not.Localization;

public static class LocalizationInjection
{
    public static IServiceCollection AddDummyLocalizer(this IServiceCollection services)
    {
        return services.AddSingleton<ILocalizer, DummyLocalizer>();
    }
}
