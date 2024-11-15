using MudBlazor;
using Not.Blazor.Components;
using Not.Services;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Pages.Dashboard.Startlist;

public class StartlistTabs : NotComponent
{
    protected MudTabs _tabs = default!;
    protected List<string> _tabHeaders = [];
    [Parameter]
    public IEnumerable<Start> Starts { get; set; } = [];
    public Dictionary<string, List<Start>> StartlistByStage { get; set; } = [];

    protected override void OnParametersSet()
    {
        foreach (var start in Starts)
        {
            var tabHeader = $"{@Localizer.Get("Stage")} {start.PhaseNumber}";
            if (!_tabHeaders.Any(t => t == tabHeader))
            {
                _tabHeaders.Add(tabHeader);
                StartlistByStage.Add(tabHeader, []);
            }
            if(!StartlistByStage[tabHeader].Any(s => s == start))
            {
                StartlistByStage[tabHeader].Add(start);
            }
        }
    }
}
