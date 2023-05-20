using Core.ConventionalServices;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Core.Application.CoreApplicationConstants;

namespace Core.Application.Services;

public class NetworkService : INetworkBroadcastService, INetworkHandshakeService
{
    private readonly IHandshakeValidatorService handshakeValidatorService;

    public NetworkService(IHandshakeValidatorService handshakeValidatorService)
    {
        this.handshakeValidatorService = handshakeValidatorService;
    }

    public Task StartBroadcasting(CancellationToken token)
    {
        using var server = new UdpClient(NETWORK_BROADCAST_PORT);
        var serverPayload = this.handshakeValidatorService.CreatePayload(Apps.JUDGE);

        while (!token.IsCancellationRequested)
        {
            var clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
            var clientPayload = server.Receive(ref clientEndpoint);
            if (this.handshakeValidatorService.ValidatePayload(clientPayload, Apps.WITNESS))
            {
                Console.WriteLine($"Handshake with '{Apps.WITNESS}' on  '{clientEndpoint.Address}'");
                server.Send(serverPayload, serverPayload.Length, clientEndpoint);
            }
            var contents = Encoding.UTF8.GetString(clientPayload);
            Console.WriteLine($"Invalid handshake attempt '{contents}' from '{clientEndpoint.Address}'");
        }
        return Task.CompletedTask;
    }

    public Task<IPAddress> Handshake(string app, CancellationToken token)
    {
        var client = new UdpClient();
        var payload = this.handshakeValidatorService.CreatePayload(app);
        var serverEndpoint = new IPEndPoint(IPAddress.Any, 0);

        while (!token.IsCancellationRequested)
        {
            client.EnableBroadcast = true;
            client.Send(payload, payload.Length, new IPEndPoint(IPAddress.Broadcast, NETWORK_BROADCAST_PORT));
            var responsePayload = client.Receive(ref serverEndpoint);
            if (this.handshakeValidatorService.ValidatePayload(responsePayload, Apps.JUDGE))
            {
                Console.WriteLine($"Handshake completed with '{Apps.JUDGE}' on '{serverEndpoint.Address}'");
                client.Close();
                return Task.FromResult(serverEndpoint.Address);
            }
        }
        return Task.FromResult((IPAddress)null!);
    }
}

public interface INetworkBroadcastService : ITransientService
{
    Task StartBroadcasting(CancellationToken token);
}

public interface INetworkHandshakeService : ITransientService
{
    Task<IPAddress> Handshake(string app, CancellationToken token);
}
