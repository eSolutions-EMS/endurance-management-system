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
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var server = new UdpClient(NETWORK_BROADCAST_PORT);
        var payload = "Huston"u8.ToArray();

        while (!stoppingToken.IsCancellationRequested)
        {
            var clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
            var clientData = server.Receive(ref clientEndpoint);
            var clientPayload = Encoding.ASCII.GetString(clientData);

            Console.WriteLine($"Received {clientPayload} from {clientEndpoint.Address}, sending response");
            server.Send(payload, payload.Length, clientEndpoint);
        }
        return Task.CompletedTask;
    }
}
