﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Not.Blazor.Mud.Extensions;
using Not.Blazor.TM.Dialogs;
using Not.Blazor.TM.Forms;

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
