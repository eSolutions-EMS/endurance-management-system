using MudBlazor;
using Not.Services;

namespace Not.Blazor.Mud.Components.Base;

public class NotButtonBase : MudButton
{
    [Inject]
    ILocalizer Localizer { get; set; } = default!;

    [Parameter]
    public string? Text { get; set; }

    protected override void OnParametersSet()
    {
        ChildContent = Text == null ? null : new RenderFragment(x => x.AddContent(0, Localizer.Get(Text)));
    }
}
