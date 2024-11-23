using Not.Blazor.Components;
using NTS.Judge.Blazor.Core.Ports;

namespace NTS.Judge.Blazor.Core.Dash.Actions.EliminationForms.Shared;

public abstract class EliminationForm : NComponent
{
    internal abstract Task Eliminate();

    [Inject]
    protected IParticipationBehind ParticipationBehind { get; set; } = default!;
    public bool IsEliminated => ParticipationBehind.SelectedParticipation?.Eliminated != null;

    internal async Task Restore()
    {
        await ParticipationBehind.RestoreQualification();
    }
}
