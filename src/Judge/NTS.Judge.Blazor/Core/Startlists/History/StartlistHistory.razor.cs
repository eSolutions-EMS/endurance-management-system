namespace NTS.Judge.Blazor.Core.Startlists.History;

public partial class StartlistHistory
{
    [Inject]
    public IStartlistHistory Behind { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Observe(Behind);
        CreateStartlistsByStage(Behind.History);
    }

    protected override void OnBeforeRender()
    {
        CreateStartlistsByStage(Behind.History);
    }
}
