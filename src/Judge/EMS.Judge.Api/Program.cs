using EMS.Judge.Api.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace EMS.Judge.Api;

public class JudgeApi
{
    public static void Start(IServiceProvider appServiceProvider)
    {
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder
                => webBuilder
                    .ConfigureServices(services 
                        => services.AddSingleton<IJudgeServiceProvider>(new JudgeServiceProvider(appServiceProvider)))
                    .UseUrls("http://*:11337")
                    .UseStartup<Startup>())
            .Build()
            .Run();
        
        Console.WriteLine("================================================");
        Console.WriteLine("=               JUDGE API running               ");
        Console.WriteLine("================================================");
    }
}
