using System.Diagnostics;
using Not.Application.RPC;
using Not.Application.RPC.SignalR;
using Not.Serialization;
using Xunit.Abstractions;

namespace Not.Tests.RPC;

public abstract class HubFixture<T> : IDisposable
    where T : ITestRpcClient
{
    readonly ITestOutputHelper _testOutputHelper;
    readonly RpcProtocol _rpcProtocol;
    readonly int _hubPort;
    readonly string _hubPattern;
    readonly string _hubExecutable;
    SignalRSocket? _socket;
    T? _client;
    Process? _hubProcess;

    public HubFixture(
        ITestOutputHelper testOutputHelper,
        RpcProtocol rpcProtocol,
        int hubPort,
        string hubPattern,
        string hubExecutable
    )
    {
        _testOutputHelper = testOutputHelper;
        _rpcProtocol = rpcProtocol;
        _hubPort = hubPort;
        _hubPattern = hubPattern;
        _hubExecutable = hubExecutable;
        Start();
    }

    protected abstract T CreateClient(SignalRSocket socket, ITestOutputHelper testOutputHelper);

    public T GetClient(ITestOutputHelper testOutputHelper)
    {
        return _client ?? CreateClient(testOutputHelper);
    }

    public void Start()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var info = new ProcessStartInfo
        {
            FileName = Path.Combine(currentDirectory, _hubExecutable),
        };

        _hubProcess = Process.Start(info);
        _testOutputHelper.WriteLine(
            $"-------- Process -------- Id: {_hubProcess?.Id}, name: {_hubProcess?.ProcessName}, exited: {_hubProcess?.HasExited}, exitCode: {_hubProcess?.ExitCode}"
        );
        var message = $"-------- Process -------- Serialized: {_hubProcess?.ToJson()}";
        _testOutputHelper.WriteLine(message);
    }

    public void Dispose()
    {
        _hubProcess?.Kill();
    }

    T CreateClient(ITestOutputHelper testOutputHelper)
    {
        _socket ??= ConfigureRpc();
        return _client = CreateClient(_socket, testOutputHelper);
    }

    SignalRSocket ConfigureRpc()
    {
        var context = new SignalRContext(_rpcProtocol, "localhost", _hubPort, _hubPattern);
        return new SignalRSocket(context);
    }
}
