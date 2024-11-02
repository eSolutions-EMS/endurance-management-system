using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.ConventionalServices;
using Core.Events;
using static Core.Application.CoreApplicationConstants;

namespace Core.Application.Services;

public class JudgeHandshakeService : INetworkBroadcastService, IHandshakeService
{
    private readonly IHandshakeValidatorService handshakeValidatorService;

    public JudgeHandshakeService(IHandshakeValidatorService handshakeValidatorService)
    {
        this.handshakeValidatorService = handshakeValidatorService;
    }

    public async Task StartBroadcasting(CancellationToken token)
    {
        var serverPayload = this.handshakeValidatorService.CreatePayload(Apps.JUDGE);
        do
        {
            using var server = new UdpClient(NETWORK_BROADCAST_PORT);
            var requestTask = server.ReceiveAsync();
            var timeout = Task.Delay(TimeSpan.FromSeconds(3));
            var first = await Task.WhenAny(requestTask, timeout);
            if (first == requestTask)
            {
                var request = await requestTask;
                if (this.handshakeValidatorService.ValidatePayload(request.Buffer, Apps.WITNESS))
                {
                    Console.WriteLine(
                        $"Handshake with '{Apps.WITNESS}' on '{request.RemoteEndPoint.Address}'"
                    );
                    server.Send(serverPayload, serverPayload.Length, request.RemoteEndPoint);
                }
                else
                {
                    var contents = Encoding.UTF8.GetString(request.Buffer);
                    Console.WriteLine(
                        $"Invalid handshake attempt '{contents}' from '{request.RemoteEndPoint.Address}'"
                    );
                }
            }
            server.Close();
        } while (!token.IsCancellationRequested);
    }

    public async Task<IPAddress> Handshake(string app, CancellationToken token)
    {
        try
        {
            var payload = this.handshakeValidatorService.CreatePayload(app);
            HandshakeResult handshake;
            do
            {
                handshake = await this.AttemptHandshake(payload);
            } while (handshake.IsTimeout && !token.IsCancellationRequested);

            var response = handshake.Result!.Value;
            if (this.handshakeValidatorService.ValidatePayload(response.Buffer, Apps.JUDGE))
            {
                Console.WriteLine(
                    $"Handshake completed with '{Apps.JUDGE}' on '{response.RemoteEndPoint.Address}'"
                );
                return response.RemoteEndPoint.Address;
            }
        }
        catch (Exception exception)
        {
            CoreEvents.RaiseError(exception);
        }

        return (IPAddress)null!;
    }

    private async Task<HandshakeResult> AttemptHandshake(byte[] payload)
    {
        using var socket = new UdpClient();
        socket.EnableBroadcast = true;
        socket.Send(
            payload,
            payload.Length,
            new IPEndPoint(IPAddress.Broadcast, NETWORK_BROADCAST_PORT)
        );

        var timeout = Task.Delay(TimeSpan.FromSeconds(3));
        var result = socket.ReceiveAsync();
        var first = await Task.WhenAny(new List<Task> { timeout, result });
        socket.Close();

        if (first is Task<UdpReceiveResult> success)
        {
            return new HandshakeResult(success.Result);
        }
        else
            return new HandshakeResult();
    }
}

public interface INetworkBroadcastService : ITransientService
{
    Task StartBroadcasting(CancellationToken token);
}

public interface IHandshakeService : ITransientService
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
