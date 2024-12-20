using NTS.Domain.Core.Aggregates;

namespace NTS.Judge.MAUI.Server.RPC.Procedures;

public interface IParticipantsClientProcedures
{
    Task ReceiveEntryUpdate(Participation entry);
}
