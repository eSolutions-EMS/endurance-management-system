using Not.Blazor.Components;
using NTS.Judge.Blazor.Core.Ports;

namespace NTS.Judge.Blazor.Core.Dash.Actions.EliminationForms.Shared;

public abstract class EliminationForm : NComponent
{
    internal abstract Task Eliminate();

    [Inject]
    protected IEliminationBehind EliminationBehind { get; set; } = default!;
    public bool IsEliminated => EliminationBehind.SelectedParticipation?.Eliminated != null;

    internal async Task Restore()
    {
        await EliminationBehind.RestoreQualification();
    }
}
