using Core.ConventionalServices;
using Core.Events;
using System;
using System.Collections.Generic;
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

    public async Task<IPAddress> Handshake(string app, CancellationToken token)
    {
        try
        {
            var payload = this.handshakeValidatorService.CreatePayload(app);
            var socket = this.OpenHandshakeSocket(payload);
            var handshake = await this.AttemptHandshake(socket);
            while (handshake.IsTimeout && !token.IsCancellationRequested)
            {
                socket.Close();
                socket = this.OpenHandshakeSocket(payload);
                handshake = await this.AttemptHandshake(socket);
            }

            var response = handshake.Result!.Value;
            if (this.handshakeValidatorService.ValidatePayload(response.Buffer, Apps.JUDGE))
            {
                Console.WriteLine($"Handshake completed with '{Apps.JUDGE}' on '{response.RemoteEndPoint.Address}'");
                socket.Close();
                return response.RemoteEndPoint.Address;
            }
        }
        catch (Exception exception)
        {
            CoreEvents.RaiseError(exception);
        }

        return (IPAddress)null!;
    }

    private UdpClient OpenHandshakeSocket(byte[] payload)
    {
        var socket = new UdpClient();
        socket.EnableBroadcast = true;
        socket.Send(payload, payload.Length, new IPEndPoint(IPAddress.Broadcast, NETWORK_BROADCAST_PORT));
        return socket;
    }

    private async Task<HandshakeResult> AttemptHandshake(UdpClient client)
    {
        var timeout = Task.Delay(TimeSpan.FromSeconds(5));
        var result = client.ReceiveAsync();
        var first = await Task.WhenAny(new List<Task> { timeout, result });
        if (first is Task<UdpReceiveResult> success)
        {
            return new HandshakeResult(success.Result);
        }
        else return new HandshakeResult();
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


internal class HandshakeResult
{
    public HandshakeResult(UdpReceiveResult? result = null)
    {
        this.Result = result;
    }

    public bool IsTimeout => this.Result == null;
    public UdpReceiveResult? Result { get; }
}