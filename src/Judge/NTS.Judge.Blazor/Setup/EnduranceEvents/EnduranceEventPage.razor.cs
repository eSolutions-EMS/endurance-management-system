using Not.Blazor.CRUD.Forms;

namespace NTS.Judge.Blazor.Setup.EnduranceEvents;

public partial class EnduranceEventPage
{
    [Inject]
    IEnduranceEventBehind Behind { get; set; } = default!;

    [Inject]
    FormManager<EnduranceEventFormModel, EnduranceEventForm> FormManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Observe(Behind);
    }

    async Task OpenCreateForm()
    {
        await FormManager.Create();
    }
}
