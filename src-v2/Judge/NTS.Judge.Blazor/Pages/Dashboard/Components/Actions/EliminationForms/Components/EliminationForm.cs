using Not.Blazor.Components.Base;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Blazor.Pages.Dashboard.Components.Actions.EliminationForms.Components;

public abstract class EliminationForm : NotComponent
{
    internal abstract Task Eliminate();

    [Inject]
    protected IParticipationBehind _participationBehind { get; set; } = default!;
    public bool IsEliminated => _participationBehind.SelectedParticipation?.Eliminated != null;

    internal async Task Restore()
    {
        await _participationBehind.RestoreQualification();
    }
}
