using MudBlazor;
using NTS.Domain.Core.Aggregates;

namespace NTS.Judge.Blazor.Core.Rankings.Menu;

public partial class RankingMenu
{
    IEnumerable<IGrouping<string, Ranking>> _rankingsByDistance = Enumerable.Empty<IGrouping<string, Ranking>>();

    [Inject]
    public IRankingBehind Behind { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Observe(Behind);

        var rankings = await Behind.GetRankings();
        _rankingsByDistance = rankings.GroupBy(x => x.Name);
    }
}
