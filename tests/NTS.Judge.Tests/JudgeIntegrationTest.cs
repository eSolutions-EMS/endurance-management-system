using Microsoft.Extensions.DependencyInjection;
using Not.Injection;
using Not.Filesystem;
using Not.Storage.Stores;
using Not.Tests;
using Not.Application.RPC;
using NTS.Application;

namespace NTS.Judge.Tests;

public abstract class JudgeIntegrationTest : IntegrationTest
{
    protected JudgeIntegrationTest(string stateFilename) : base(stateFilename)
    {
    }

    protected override IServiceCollection ConfigureServices(string storagePath)
    {
        FileContextHelper.SetDebugRootDirectory("nts");
        var services = new ServiceCollection();
        return services
            .ConfigureJudge()
            .AddRpcSocket(RpcProtocol.Http, "localhost", ApplicationConstants.RPC_PORT, ApplicationConstants.JUDGE_HUB)
            .AddJsonFileStore(x => x.Path = storagePath)
            .AddStaticOptionsStore(x => x.Path = FileContextHelper.GetAppDirectory("resources"))
            .RegisterConventionalServices();
    }
}
