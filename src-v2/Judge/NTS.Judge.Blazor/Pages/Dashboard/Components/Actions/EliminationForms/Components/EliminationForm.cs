using NTS.Judge.Blazor.Ports;
using Not.Blazor.Components;

namespace NTS.Judge.Blazor.Pages.Dashboard.Components.Actions.EliminationForms;

public abstract class EliminationForm : NotComponent
{
    [Inject]
    protected IParticipationBehind _participationBehind { get; set; } = default!;

    [Parameter]
    public EventCallback OnRestore { get; set; }
    [Parameter]
    public EventCallback OnEliminate { get; set; }

    protected bool IsEliminated { get; private set; }

    protected abstract Task EliminateAction();

    internal async Task Eliminate()
    {
        await EliminateAction();
        await OnEliminate.InvokeAsync();
        IsEliminated = true;
    }

    internal async Task Restore()
    {
        await _participationBehind.RestoreQualification();
        await OnRestore.InvokeAsync();
        IsEliminated = false;
    }
}

