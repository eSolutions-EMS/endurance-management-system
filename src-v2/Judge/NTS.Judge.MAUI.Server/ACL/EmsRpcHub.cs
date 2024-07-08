using Microsoft.AspNetCore.SignalR;
using NTS.Judge.MAUI.Server.ACL.EMS;

namespace NTS.Judge.MAUI.Server.ACL;

public class EmsRpcHub : Hub<IEmsClientProcedures>, IEmsStartlistHubProcedures, IEmsEmsParticipantstHubProcedures
{
    public Dictionary<int, EmsStartlist> SendStartlist()
    {
        //var startlist = this.managerRoot.GetStartList();
        return startlist;
    }

    public EmsParticipantsPayload SendParticipants()
    {
        // TODO: create ParticipantsPayload
        return new EmsParticipantsPayload
        {
            Participants = participants.ToList(),
            EventId = eventId,
        };
    }
    public Task ReceiveWitnessEvent(IEnumerable<EmsParticipantEntry> entries, EmsWitnessEventType type)
    {
        // Task.Run because Event hadling in dotnet seems to hold the current thread. Further investigation is needed
        // but what was happening is that Witness apps didn't receive rpc response untill the handling thread was finished
        // which is motly visible when it causes a validation (popup) which blocks the thread until closed in Prism/WPF
        Task.Run(() =>
        {
            foreach (var entry in entries)
            {
                //TODO: raise Participation event
            }
        });

        return Task.CompletedTask;
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"Connected: {Context.ConnectionId}");
        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine($"Disconnected: {Context.ConnectionId}");
        return Task.CompletedTask;
    }

    public class ClientService : IDisposable
    {
        private readonly IHubContext<EmsRpcHub, IEmsClientProcedures> _hub;

        public ClientService(
            IHubContext<EmsRpcHub, IEmsClientProcedures> hub)
        {
            _hub = hub;
            // TODO attach event handlers
        }

        public void SendStartlistEntryUpdate(object? _, (string Number, EmsCollectionAction Action) args)
        {
            //var entry = this._managerRoot.GetStarlistEntry(args.Number);
            if (entry == null)
            {
                return;
            }
            _hub.Clients.All.ReceiveEntry(entry, args.Action);
        }

        public void SendParticipantEntryUpdate(object? _, (string Number, EmsCollectionAction Action) args)
        {
            //var entry = this._managerRoot.GetParticipantEntry(args.Number);
            if (entry == null)
            {
                return;
            }
            _hub.Clients.All.ReceiveEntryUpdate(entry, args.Action);
        }

        public void Dispose()
        {
            // TODO: Dispose
        }
    }
}