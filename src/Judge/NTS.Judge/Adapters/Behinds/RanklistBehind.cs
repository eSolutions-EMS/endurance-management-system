using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
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

    public async Task<IEnumerable<Ranking>> GetRankings()
    {
        return await _rankings.ReadAll();
    }

    public async Task SelectRanking(int id)
    {
        var ranking = await _rankings.Read(id);
        GuardHelper.ThrowIfDefault(ranking);

        Ranklist = await CreateRanklist(ranking);
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
        Ranklist = await CreateRanklist(ranking);
    }

    private async Task<Ranklist> CreateRanklist(Ranking ranking)
    {
        var participations = await _participations.ReadAll();
        return new Ranklist(ranking, participations);
    }
}
