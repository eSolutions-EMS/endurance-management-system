using Core.Services;
using Core.Utilities;
using System;

namespace EMS.Judge.Api;

public class ApiInitializer : IInitializer
{
    private readonly IServiceProvider serviceProvider;

    public ApiInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public int RunningOrder => 20;
    public void Run()
    {
        StaticProvider.Initialize(this.serviceProvider);
    }
}