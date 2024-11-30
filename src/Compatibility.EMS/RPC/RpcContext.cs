using System;

namespace NTS.Judge.Tests.RPC;

public class RpcContext
{
    private readonly RpcProtocls protocol;
    private readonly int port;
    private readonly string endpoint;
    private string? host;

    public RpcContext(RpcProtocls protocol, int port, string endpoint)
    {
        if (endpoint.StartsWith("/"))
        {
            endpoint = endpoint[1..];
        }
        this.protocol = protocol;
        this.port = port;
        this.endpoint = endpoint;
    }

    public string? Host
    {
        get => host;
        internal set
        {
            if (value == null)
            {
                throw new ArgumentException("RPC host cannot be null", nameof(Host));
            }
            if (value.EndsWith("/") || value.EndsWith(":"))
            {
                host = value[..^1];
            }
            else
            {
                host = value;
            }
        }
    }
    public string? Url
        => Host == null
            ? null
            : $"{protocol.ToString().ToLower()}://{host}:{port}/{endpoint}";
}
