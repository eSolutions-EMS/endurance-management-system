using Not.Blazor.Components;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Core.Startlists;

public abstract class StartlistTabs : NComponent
{
    protected Dictionary<string, List<Start>> StartlistsByStage { get; set; } = [];

    protected virtual string[] TableHeaders { get; } = ["Number", "Athlete", "Loop", "Start Time"];

    protected void CreateStartlistsByStage(IEnumerable<Start> starts)
    {
        foreach (var start in starts)
        {
            var tabHeader = $"{Localizer.Get("Stage")} {start.PhaseNumber}";
            if (!StartlistsByStage.Keys.Any(t => t == tabHeader))
            {
                StartlistsByStage.Add(tabHeader, []);
            }
            if (!StartlistsByStage[tabHeader].Any(s => s.Number == start.Number))
            {
                StartlistsByStage[tabHeader].Add(start);
            }
        }
    }
}
