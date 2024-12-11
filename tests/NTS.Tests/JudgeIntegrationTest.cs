using Microsoft.Extensions.DependencyInjection;
using Not.Injection;
using Not.Filesystem;
using Not.Storage.Stores;
using Not.Tests;

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
            .AddJsonFileStore(x => x.Path = storagePath)
            .AddStaticOptionsStore(x => x.Path = FileContextHelper.GetAppDirectory("resources"))
            .RegisterConventionalServices();
    }
}
