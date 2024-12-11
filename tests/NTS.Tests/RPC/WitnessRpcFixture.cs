using Not.Application.RPC;
using Not.Application.RPC.SignalR;
using Not.Tests.RPC;
using NTS.Application;

namespace NTS.Judge.Tests.RPC;

public class WitnessRpcFixture : HubFixture<WitnessTestClient>
{
    public WitnessRpcFixture() : base(RpcProtocol.Http, ApplicationConstants.RPC_PORT, ApplicationConstants.WITNESS_HUB, "NTS.Judge.MAUI.Server.exe")
    {
    }

    protected override WitnessTestClient CreateClient(SignalRSocket socket)
    {
        return new WitnessTestClient(socket);
    }
}

[CollectionDefinition(nameof(WitnessRpcFixture))]
public class HubFixtureCollection : ICollectionFixture<WitnessRpcFixture>
{
}

