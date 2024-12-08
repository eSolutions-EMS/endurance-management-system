using Microsoft.Extensions.DependencyInjection;
using Not.Application.RPC;
using NTS.Application;
using NTS.Domain.Objects;
using NTS.Judge.Core;
using NTS.Judge.Core.Behinds.Adapters;
using NTS.Storage.Core;

namespace NTS.Judge.Tests;

[Collection(nameof(HubFixture))]
public class RpcIntegrationTest : JsonFileStoreIntegrationTest
{
    private readonly HubFixture _hubFixture;

    public RpcIntegrationTest(HubFixture hubFixture) : base(nameof(CoreState))
    {
        _hubFixture = hubFixture;
    }

    protected override IServiceCollection ConfigureServices(string storagePath)
    {
        return base.ConfigureServices(storagePath)
            .ConfigureRpc(RpcProtocol.Http, "localhost", ApplicationConstants.RPC_PORT, ApplicationConstants.JUDGE_HUB);
    }

    [Fact]
    public async Task TestEliminatedOnRpcClient()
    {
        await SeedResource("55-present.json");

        var now = DateTimeOffset.Now;
        var time = new DateTimeOffset(now.Year, now.Month, now.Day, 22, 17, 31, now.Offset);
        var timestamp = new Timestamp(time);
        var snapshot = new Snapshot(55, Domain.Enums.SnapshotType.Vet, Domain.Enums.SnapshotMethod.Manual, timestamp);

        var client = _hubFixture.GetClient();
        var processor = Provider.GetRequiredService<ISnapshotProcessor>();

        await ((ParticipationBehind)processor).Initialize([]);

        await client.Connect();
        await processor.Process(snapshot);
        await Task.Delay(TimeSpan.FromSeconds(5));
        var isInvoked = client.InvokedMethods.Contains(nameof(client.ReceiveEntryUpdate));
        Assert.True(isInvoked);
    }
}
