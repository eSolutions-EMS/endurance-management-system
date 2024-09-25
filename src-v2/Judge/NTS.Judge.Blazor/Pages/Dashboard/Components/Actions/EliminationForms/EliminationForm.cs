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

    public abstract Task Eliminate();

    public async Task Restore()
    {
        await OnRestore.InvokeAsync();
    }
}

