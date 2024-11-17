using Not.Application.Behinds.Adapters;
using Not.Application.CRUD.Ports;
using Not.Exceptions;
using Not.Safe;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Domain.Core.Objects.Payloads;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class RanklistBehind : ObservableBehind, IRanklistBehind
{
    readonly IRepository<Ranking> _rankings;

    public RanklistBehind(IRepository<Ranking> rankings)
    {
        _rankings = rankings;
        Participation.PHASE_COMPLETED_EVENT.Subscribe(UpdateRanklist);
        Participation.ELIMINATED_EVENT.Subscribe(UpdateRanklist);
        Participation.RESTORED_EVENT.Subscribe(UpdateRanklist);
    }

    public Ranklist? Ranklist { get; private set; }

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        var ranking = await _rankings.Read(x => true);
        if (ranking == null)
        {
            return false;
        }
        Ranklist = new Ranklist(ranking);
        return true;
    }

    public async Task<IEnumerable<Ranking>> GetRankings()
    {
        return await SafeHelper.Run(SafeGetRankings) ?? [];
    }

    public async Task SelectRanking(int id)
    {
        Task action() => SafeSelectRanking(id);
        await SafeHelper.Run(action);
    }

    async Task<IEnumerable<Ranking>> SafeGetRankings()
    {
        return await _rankings.ReadAll();
    }

    async Task SafeSelectRanking(int id)
    {
        var ranking = await _rankings.Read(id);
        GuardHelper.ThrowIfDefault(ranking);

        Ranklist = new Ranklist(ranking);
        EmitChange();
    }

    void UpdateRanklist(ParticipationPayload payload)
    {
        Ranklist?.Update(payload.Participation);
    }
}
