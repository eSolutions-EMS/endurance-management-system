using MudBlazor;
using NTS.Domain.Core.Aggregates;

namespace NTS.Judge.Blazor.Core.Rankings.Menu;

public partial class RankingMenu
{
    List<(string Name, IOrderedEnumerable<Ranking> ranking)> _rankingsByDistance = [];

    [Inject]
    public IRankingBehind Behind { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Observe(Behind);

        var rankings = await Behind.GetRankings();
        // TODO: this is too complicated, probably should be in the behind or better yet in the Domain Model
        _rankingsByDistance = rankings
            .GroupBy(x => x.Name)
            .Select(x => (x.Key, x.Where(x => x.Entries.Any()).OrderBy(x => x.Category)))
            .ToList();
    }
}
