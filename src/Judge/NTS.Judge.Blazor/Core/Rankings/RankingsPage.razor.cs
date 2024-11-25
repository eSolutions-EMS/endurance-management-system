namespace NTS.Judge.Blazor.Core.Rankings;

public partial class RankingsPage
{
    bool _showProtocol;

    [Inject]
    IRankingBehind Behind { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Observe(Behind);
    }

    void ShowProtocol()
    {
        _showProtocol = true;
    }

    void ShowRanklist()
    {
        _showProtocol = false;
    }
}
