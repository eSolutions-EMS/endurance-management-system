namespace NTS.Judge.Blazor.Shared.Components.SidePanels;
public partial class SidePanel
{
    [Inject]
    ICoreBehind CoreBehind { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Observe(CoreBehind);
    }

    public async void Start()
    {
        await CoreBehind.Start();
    }
}
