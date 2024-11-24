using Not.Blazor.Components;
using NTS.Judge.Blazor.Core.Ports;

namespace NTS.Judge.Blazor.Core.Dash.Actions.EliminationForms.Shared;

public abstract class EliminationForm : NComponent
{
    internal abstract Task Eliminate();

    [Inject]
    protected IEliminationsBehind Eliminations { get; set; } = default!;
    public bool IsEliminated => Eliminations.SelectedParticipation?.Eliminated != null;

    internal async Task Restore()
    {
        await Eliminations.RestoreQualification();
    }
}
