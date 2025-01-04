using Not.Application.RPC.Clients;
using Not.Application.RPC.SignalR;
using Not.Tests.RPC;
using NTS.ACL.Entities;
using NTS.ACL.Enums;
using NTS.ACL.RPC;
using NTS.ACL.RPC.Procedures;
using Xunit.Abstractions;

namespace NTS.Judge.Tests;

public class WitnessTestClient
    : RpcClient,
        IEmsParticipantsClientProcedures,
        IEmsStartlistClientProcedures,
        ITestRpcClient
{
    private readonly ITestOutputHelper _testOutputHelper;

    public WitnessTestClient(IRpcSocket socket, ITestOutputHelper testOutputHelper)
        : base(socket)
    {
        RegisterClientProcedure<EmsStartlistEntry, EmsCollectionAction>(
            nameof(ReceiveEntry),
            ReceiveEntry
        );
        RegisterClientProcedure<EmsParticipantEntry, EmsCollectionAction>(
            nameof(ReceiveEntryUpdate),
            ReceiveEntryUpdate
        );
        _testOutputHelper = testOutputHelper;
    }

    public int Id { get; }
    public List<string> InvokedMethods { get; } = [];

    public void Dispose()
    {
        // Reset the invoked methods after each test
        InvokedMethods.Clear();
    }

    public override async Task Connect()
    {
        await base.Connect();
        _testOutputHelper.WriteLine($"-------- RPC ------- Connection: {Socket.IsConnected}");
        _testOutputHelper.WriteLine($"-------- RPC ------- Procedures: {string.Join(Environment.NewLine, Socket.Procedures)}");
    }

    public Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action)
    {
        _testOutputHelper.WriteLine($"-------- RPC --------- Received '{nameof(ReceiveEntry)}'");
        InvokedMethods.Add(nameof(ReceiveEntry));
        return Task.CompletedTask;
    }

    public Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action)
    {
        _testOutputHelper.WriteLine(
            $"-------- RPC --------- Received '{nameof(ReceiveEntryUpdate)}'"
        );
        InvokedMethods.Add(nameof(ReceiveEntryUpdate));
        return Task.CompletedTask;
    }
}
