using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class RanklistBehind : ObservableBehind, IRanklistBehind
{
    private readonly IRepository<Ranking> _rankings;

    public RanklistBehind(IRepository<Ranking> rankings)
    {
        _rankings = rankings;
    }

    public Ranklist? Ranklist { get; private set; }

    public async Task<IEnumerable<Ranking>> GetRankings()
    {
        return await _rankings.ReadAll();
    }

    public async Task SelectRanking(int id)
    {
        var ranking = await _rankings.Read(id);
        GuardHelper.ThrowIfDefault(ranking);

        Ranklist = new Ranklist(ranking);
        EmitChange();
    }

    // This isn't currently used. Consider wheather or not Initialize should be part of IObservableBehind
    public override async Task Initialize()
    {
        var ranking = await _rankings.Read(x => true);
        if (ranking == null)
        {
            return;
        }
        Ranklist = new Ranklist(ranking);
    }
}
