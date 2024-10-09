using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
using Not.Safe;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class RanklistBehind : ObservableBehind, IRanklistBehind
{
    private readonly IRepository<Ranking> _rankings;
    private readonly IRepository<Participation> _participations;

    public RanklistBehind(IRepository<Ranking> rankings, IRepository<Participation> participations)
    {
        _rankings = rankings;
        _participations = participations;
    }

    public Ranklist? Ranklist { get; private set; }

    protected override async Task<bool> PerformInitialization()
    {
        var ranking = await _rankings.Read(x => true);
        if (ranking == null)
        {
            return false;
        }
        Ranklist = await CreateRanklist(ranking);
        return true;
    }

    async Task<IEnumerable<Ranking>> SafeGetRankings()
    {
        return await _rankings.ReadAll();
    }

    async Task SafeSelectRanking(int id)
    {
        var ranking = await _rankings.Read(id);
        GuardHelper.ThrowIfDefault(ranking);

        Ranklist = await CreateRanklist(ranking);
        EmitChange();
    }

    async Task<Ranklist> CreateRanklist(Ranking ranking)
    {
        var participations = await _participations.ReadAll();
        return new Ranklist(ranking, participations);
    }

    #region SafePattern
    
    public async Task<IEnumerable<Ranking>> GetRankings()
    {
        return await SafeHelper.Run(SafeGetRankings) ?? [];
    }

    public async Task SelectRanking(int id)
    {
        await SafeHelper.Run(() => SafeSelectRanking(id));
    }

    #endregion
}
