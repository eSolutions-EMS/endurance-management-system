using System.Diagnostics;
using Not.Application.RPC;
using Not.Application.RPC.SignalR;

namespace Not.Tests.RPC;

public abstract class HubFixture<T> : IDisposable
    where T : ITestRpcClient
{
    readonly RpcProtocol _rpcProtocol;
    readonly int _hubPort;
    readonly string _hubPattern;
    readonly string _hubExecutable;
    SignalRSocket? _socket;
    T? _client;
    Process? _hubProcess;

    public HubFixture(RpcProtocol rpcProtocol, int hubPort, string hubPattern, string hubExecutable)
    {
        _rpcProtocol = rpcProtocol;
        _hubPort = hubPort;
        _hubPattern = hubPattern;
        _hubExecutable = hubExecutable;
        Start();
    }

    protected abstract T CreateClient(SignalRSocket socket);

    public T GetClient()
    {
        return _client ?? CreateClient();
    }

    public void Start()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var info = new ProcessStartInfo
        {
            FileName = Path.Combine(currentDirectory, _hubExecutable),
        };

        _hubProcess = Process.Start(info);
    }

    public void Dispose()
    {
        _hubProcess?.Kill();
    }

    T CreateClient()
    {
        _socket ??= ConfigureRpc();
        return _client = CreateClient(_socket);
    }

    SignalRSocket ConfigureRpc()
    {
        var context = new SignalRContext(_rpcProtocol, "localhost", _hubPort, _hubPattern);
        return new SignalRSocket(context);
    }
}
