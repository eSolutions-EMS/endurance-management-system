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

    protected override async Task PerformInitialization()
    {
        var ranking = await _rankings.Read(x => true);
        if (ranking == null)
        {
            return;
        }
        Ranklist = await CreateRanklist(ranking);
    }

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

    private async Task<Ranklist> CreateRanklist(Ranking ranking)
    {
        var participations = await _participations.ReadAll();
        return new Ranklist(ranking, participations);
    }
}
