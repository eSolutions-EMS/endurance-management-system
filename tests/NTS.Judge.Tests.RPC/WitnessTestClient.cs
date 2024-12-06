using NTS.ACL.Entities;
using NTS.ACL.Enums;
using NTS.ACL.RPC;
using NTS.ACL.RPC.Procedures;

namespace NTS.ACL.RPC;

public class WitnessTestClient : RpcClient, IEmsParticipantsClientProcedures, IEmsStartlistClientProcedures
{
    public WitnessTestClient(SignalRSocket socket) : base(socket)
    {
        RegisterClientProcedure<EmsStartlistEntry, EmsCollectionAction>(nameof(ReceiveEntry), ReceiveEntry);
        RegisterClientProcedure<EmsParticipantEntry, EmsCollectionAction>(nameof(ReceiveEntryUpdate), ReceiveEntryUpdate);
    }

    public Task ReceiveEntry(EmsStartlistEntry entry, EmsCollectionAction action)
    {
        throw new NotImplementedException();
    }

    public Task ReceiveEntryUpdate(EmsParticipantEntry entry, EmsCollectionAction action)
    {
        throw new NotImplementedException();
    }
}
