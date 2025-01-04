using NTS.Domain.Enums;
using NTS.Domain.Objects;
using NTS.Judge.Core;
using NTS.Judge.Tests.Helpers;
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

        var timestamp = TimestampHelper.Create(hour: 19);
        var snapshot = new Snapshot(1337, SnapshotType.Stage, SnapshotMethod.Manual, timestamp);

        var processor = await GetBehind<ISnapshotProcessor>();

        await AssertRpcInvoked(
            _witnessFIxture, 
            () => processor.Process(snapshot),
            nameof(WitnessTestClient.ReceiveEntryUpdate));
    }
}
