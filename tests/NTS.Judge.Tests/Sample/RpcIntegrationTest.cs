using NTS.Domain.Objects;
using NTS.Judge.Core;
using NTS.Storage.Core;

namespace NTS.Judge.Tests.Sample;

[Collection(nameof(WitnessRpcFixture))]
public class RpcIntegrationTest : JudgeIntegrationTest
{
    private readonly WitnessRpcFixture _witnessFIxture;

    public RpcIntegrationTest(WitnessRpcFixture witnessFixture) : base(nameof(CoreState))
    {
        _witnessFIxture = witnessFixture;
    }


    [Fact]
    public async Task TestEliminatedOnRpcClient()
    {
        await Seed();

        var now = DateTimeOffset.Now;
        var time = new DateTimeOffset(now.Year, now.Month, now.Day, 22, 17, 31, now.Offset);
        var timestamp = new Timestamp(time);
        var snapshot = new Snapshot(55, Domain.Enums.SnapshotType.Vet, Domain.Enums.SnapshotMethod.Manual, timestamp);

        var processor = await GetBehind<ISnapshotProcessor>();

        await AssertRpcInvoked(
            _witnessFIxture, 
            () => processor.Process(snapshot),
            nameof(WitnessTestClient.ReceiveEntryUpdate));
    }
}
