using NTS.ACL.RPC.Procedures;

namespace NTS.Application.RPC;

public interface IWitnessClientProcedures 
    : IEmsParticipantsClientProcedures,
        IEmsParticipantsHubProcedures,
        IEmsStartlistClientProcedures,
        IHubProcedures
{ }
