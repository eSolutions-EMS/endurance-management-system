using MudBlazor;
using Not.Blazor.Components;
using Not.Services;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Pages.Dashboard.Startlist;

public abstract class StartlistTabs : NotComponent
{
    protected abstract IEnumerable<Start> _starts { get; }
    protected Dictionary<string, List<Start>> _startlistByStage { get; set; } = [];

    protected void CreateHeadersAndGroupByStage()
    {
        foreach (var start in _starts)
        {
            var tabHeader = $"{@Localizer.Get("Stage")} {start.PhaseNumber}";
            if (!_startlistByStage.Keys.Any(t => t == tabHeader))
            {
                _startlistByStage.Add(tabHeader, []);
            }
            if (!_startlistByStage[tabHeader].Any(s => s.Number == start.Number))
            {
                _startlistByStage[tabHeader].Add(start);
            }
        }
    }
}
