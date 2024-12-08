using System.Diagnostics;
using Not.Application.RPC;
using Not.Application.RPC.SignalR;
using NTS.ACL.RPC;
using NTS.Application;

namespace NTS.Judge.Tests;

public class HubFixture : IDisposable
{
    SignalRSocket? _socket;
    Process? _hubProcess;

    public HubFixture()
    {
        Start();
    }

    public WitnessTestClient GetClient()
    {
        _socket ??= ConfigureRpc();
        return new WitnessTestClient(_socket);
    }

    SignalRSocket ConfigureRpc()
    {
        var context = new SignalRContext(RpcProtocol.Http, "localhost", ApplicationConstants.RPC_PORT, CoreApplicationConstants.RPC_ENDPOINT);
        return new SignalRSocket(context);
    }

    public void Start()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var info = new ProcessStartInfo
        {
            FileName = Path.Combine(currentDirectory, "NTS.Judge.MAUI.Server.exe"),
        };

        _hubProcess = Process.Start(info);
    }

    public void Dispose()
    {
        _hubProcess?.Kill();
    }
}

[CollectionDefinition(nameof(HubFixture))]
public class HubFixtureCollection : ICollectionFixture<HubFixture>
{
}
