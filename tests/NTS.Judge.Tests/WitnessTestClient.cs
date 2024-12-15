using Not.Application.RPC.Clients;
using Not.Application.RPC.SignalR;
using Not.Tests.RPC;
using NTS.ACL.Entities;
using NTS.ACL.Enums;
using NTS.ACL.RPC;
using NTS.ACL.RPC.Procedures;

namespace NTS.Judge.Tests;

public class WitnessTestClient : RpcClient, IEmsParticipantsClientProcedures, IEmsStartlistClientProcedures, ITestRpcClient
{
    ITestRpcClient _thisTestClient;

    public WitnessTestClient(IRpcSocket socket) : base(socket)
    {
        RegisterClientProcedure<EmsStartlistEntry, EmsCollectionAction>(nameof(ReceiveEntry), ReceiveEntry);
        RegisterClientProcedure<EmsParticipantEntry, EmsCollectionAction>(nameof(ReceiveEntryUpdate), ReceiveEntryUpdate);
        _thisTestClient = this;
    }

    List<string> ITestRpcClient.InvokedMethods { get; } = [];

    public Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action)
    {
        _thisTestClient.InvokedMethods.Add(nameof(ReceiveEntry));
        return Task.CompletedTask;
    }

    public Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action)
    {
        _thisTestClient.InvokedMethods.Add(nameof(ReceiveEntryUpdate));
        return Task.CompletedTask;
    }
}

