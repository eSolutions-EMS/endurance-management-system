namespace NTS.Application.RPC;

public class RpcContext
{
    readonly RpcProtocls _protocol;
    readonly int _port;
    readonly string _endpoint;
    string? _host;

    public RpcContext(RpcProtocls protocol, int port, string endpoint)
    {
        if (endpoint.StartsWith("/"))
        {
            endpoint = endpoint[1..];
        }
        _protocol = protocol;
        _port = port;
        _endpoint = endpoint;
    }

    public string? Host
    {
        get => _host;
        internal set
        {
            if (value == null)
            {
                throw new ArgumentException("RPC host cannot be null", nameof(Host));
            }
            if (value.EndsWith("/") || value.EndsWith(":"))
            {
                _host = value[..^1];
            }
            else
            {
                _host = value;
            }
        }
    }
    
    public string? Url
        => Host == null
            ? null
            : $"{_protocol.ToString().ToLower()}://{_host}:{_port}/{_endpoint}";
}
