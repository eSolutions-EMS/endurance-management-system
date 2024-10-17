using Microsoft.AspNetCore.SignalR;
using NTS.Compatibility.EMS.Entities;

namespace NTS.Console;

public class Kur : Hub
{
    public Dictionary<int, EmsStartlist> SendStartlist()
    {
        return [];
    }

    public async Task<EmsParticipantsPayload> SendParticipants()
    {
        return new EmsParticipantsPayload
        {
            EventId = 15,
            Participants = [],
        };
    }

    public override Task OnConnectedAsync()
    {
        System.Console.WriteLine("Connected");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        System.Console.WriteLine("Disconnected");
        return base.OnDisconnectedAsync(exception);
    }
}

public class Service
{
    private readonly IHubContext<Kur> _kur;

    public Service(IHubContext<Kur> kur)
    {
        _kur = kur;
    }

    public async Task Ping()
    {
        await _kur.Clients.All.SendAsync("Ping");
    }
}
