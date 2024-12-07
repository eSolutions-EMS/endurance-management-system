using NTS.ACL.Entities;
using NTS.ACL.Enums;
using NTS.ACL.RPC;
using NTS.ACL.RPC.Procedures;
using NTS.Application;

namespace NTS.Judge.Tests.RPC;

public class WitnessTestClient : RpcClient, IEmsParticipantsClientProcedures, IEmsStartlistClientProcedures
{
    public WitnessTestClient() : base(NtsApplicationConstants.WITNESS_HUB)
    {
        RegisterClientProcedure<EmsStartlistEntry, EmsCollectionAction>(nameof(ReceiveEntry), ReceiveEntry);
        RegisterClientProcedure<EmsParticipantEntry, EmsCollectionAction>(nameof(ReceiveEntryUpdate), ReceiveEntryUpdate);
    }

    public Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action)
    {
        return Task.CompletedTask;
    }

    public Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action)
    {
        return Task.CompletedTask;
    }
}
