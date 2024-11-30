using NTS.Compatibility.EMS.Entities;
using NTS.Compatibility.EMS.Enums;
using NTS.Compatibility.EMS.RPC;
using NTS.Compatibility.EMS.RPC.Procedures;

namespace NTS.Judge.Tests.RPC;

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
