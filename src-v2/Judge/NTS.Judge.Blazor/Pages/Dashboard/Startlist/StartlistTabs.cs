using MudBlazor;
using Not.Blazor.Components;
using Not.Services;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Pages.Dashboard.Startlist;

public abstract class StartlistTabs : NotComponent
{
    public abstract IEnumerable<Start> Starts { get; }
    public Dictionary<string, List<Start>> StartlistByStage { get; set; } = [];
    protected void CreateHeadersAndGroupByStage()
    {
        foreach (var start in Starts)
        {
            var tabHeader = $"{@Localizer.Get("Stage")} {start.PhaseNumber}";
            if (!StartlistByStage.Keys.Any(t => t == tabHeader))
            {
                StartlistByStage.Add(tabHeader, []);
            }
            if (!StartlistByStage[tabHeader].Any(s => s.Number == start.Number))
            {
                StartlistByStage[tabHeader].Add(start);
            }
        }
    }
}
