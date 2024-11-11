using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Not.Blazor.CRUD.Forms;
using Not.Blazor.Dialogs;
using Not.Blazor.Mud.Extensions;

namespace Not.Blazor.Injection;

public static class BlazorInjectionExtensions
{
    public static IServiceCollection AddNotBlazor(
        this IServiceCollection services,
        IConfiguration _
    )
    {
        return services
            .AddNotMudBlazor()
            .AddTransient(typeof(DialogTM<,>))
            .AddTransient(typeof(FormManager<,>));
    }
}
