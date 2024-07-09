using NTS.Domain.Core.Events;
using NTS.Domain.Core.Objects;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.Ports;

namespace NTS.Judge.MAUI.Server.RPC;

public class SignalRHub : IRemoteProcedures
{
    public SignalRHub()
    {
    }

    public async Task ReceiveSnapshot(Snapshot snapshot)
    {
        throw new NotImplementedException();
    }

    public Task SendStartCreated(PhaseStart startCreated)
    {
        throw new NotImplementedException();
    }

    public Task SendQualificationRevoked(QualificationRevoked revoked)
    {
        throw new NotImplementedException();
    }

    public Task SendQualificationRestored(QualificationRestored restored)
    {
        throw new NotImplementedException();
    }
}
