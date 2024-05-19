using NTS.Domain.Core.Events;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.Ports;

namespace NTS.Judge.MAUI.Server;

public class SignalRHub : IRpcHub
{
    private readonly IParticipationBehind _coreBehind;

    public SignalRHub(IParticipationBehind coreBehind)
    {
        _coreBehind = coreBehind;
    }

    public async Task ReceiveSnapshot(Snapshot snapshot)
    {
        await _coreBehind.Process(snapshot);
    }

    public Task SendStartCreated(StartCreated startCreated)
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
