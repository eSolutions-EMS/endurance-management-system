namespace Not.Application.RPC.SignalR;

public class SignalRContext
{
    readonly RpcProtocol _protocol;
    readonly int _port;
    readonly string _hubPattern;
    string _host;

    public SignalRContext(RpcProtocol protocol, string host, int port, string hubPattern)
    {
        _protocol = protocol;
        _host = NormalizeHost(host);
        _port = port;
        _hubPattern = NormalizePattern(hubPattern);
    }

    public string Url => $"{_protocol.ToString().ToLower()}://{_host}:{_port}/{_hubPattern}";

    string NormalizePattern(string hubPattern)
    {
        if (hubPattern.StartsWith('/'))
        {
            hubPattern = hubPattern[1..];
        }
        return hubPattern;
    }

    string NormalizeHost(string host)
    {
        if (host.EndsWith('/') || host.EndsWith(':'))
        {
            host = host[..^1];
        }
        return host;
    }
}
