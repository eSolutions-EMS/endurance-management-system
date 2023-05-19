using Core.Application.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Judge.Api.Services;

public class NetworkBroadcastService : BackgroundService
{
    private readonly IHandshakeService handshakeService;
    public NetworkBroadcastService(IHandshakeService handshakeService)
    {
        this.handshakeService = handshakeService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Broadcasting");
        try
        {
            using var server = new UdpClient(NETWORK_BROADCAST_PORT);
            var serverPayload = this.handshakeService.CreatePayload(Apps.JUDGE);

            while (!stoppingToken.IsCancellationRequested)
            {
                var clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
                var clientPayload = server.Receive(ref clientEndpoint);
                if (this.handshakeService.ValidatePayload(clientPayload, Apps.WITNESS))
                {
                    Console.WriteLine($"Handshake with '{Apps.WITNESS}' on  '{clientEndpoint.Address}'");
                    server.Send(serverPayload, serverPayload.Length, clientEndpoint);
                }
                var contents = Encoding.UTF8.GetString(clientPayload);
                Console.WriteLine($"Invalid handshake attempt '{contents}' from '{clientEndpoint.Address}'");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error while broadcasting!");
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.StackTrace);
        }
        return Task.CompletedTask;

    }
}
