using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

namespace Not.Blazor.Mud.Extensions;

public static class MudServiceCollectionExtensions
{
    public static IServiceCollection AddNotMudBlazor(
        this IServiceCollection services,
        Action<MudServicesConfiguration>? customConfiguration = null)
    {
        return services
            .AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000; // seconds
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
                config.SnackbarConfiguration.HideTransitionDuration = 200;
                config.SnackbarConfiguration.ShowTransitionDuration = 200;

                if (customConfiguration != null)
                {
                    customConfiguration(config);
                }
            });
    }
}
