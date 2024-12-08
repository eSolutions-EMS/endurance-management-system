using Not.Application.RPC.Clients;
using Not.Application.RPC.SignalR;
using NTS.ACL.Entities;
using NTS.ACL.Enums;
using NTS.ACL.RPC;
using NTS.ACL.RPC.Procedures;

namespace NTS.Judge.Tests;

public class WitnessTestClient : RpcClient, IEmsParticipantsClientProcedures, IEmsStartlistClientProcedures
{
    public WitnessTestClient(IRpcSocket socket) : base(socket)
    {
        RegisterClientProcedure<EmsStartlistEntry, EmsCollectionAction>(nameof(ReceiveEntry), ReceiveEntry);
        RegisterClientProcedure<EmsParticipantEntry, EmsCollectionAction>(nameof(ReceiveEntryUpdate), ReceiveEntryUpdate);
    }

    public List<string> InvokedMethods { get; } = [];

    public Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action)
    {
        InvokedMethods.Add(nameof(ReceiveEntry));
        return Task.CompletedTask;
    }

    public Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action)
    {
        InvokedMethods.Add(nameof(ReceiveEntryUpdate));
        return Task.CompletedTask;
    }
}
