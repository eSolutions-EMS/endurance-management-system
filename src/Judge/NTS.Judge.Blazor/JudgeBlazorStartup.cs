using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Not.Blazor.Injection;
using NTS.Application;
using NTS.Application.RPC;

namespace NTS.Judge.Blazor;

public static class JudgeBlazorStartup
{
    public static IServiceCollection AddJudgeBlazor(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddLocalization(x => x.ResourcesPath = "Resources/Localization")
            .AddNotBlazor(configuration)
            .ConfigureRpcSocket(RpcProtocl.Http, "localhost", NtsApplicationConstants.RPC_PORT, NtsApplicationConstants.JUDGE_HUB);

        return services;
    }
}
