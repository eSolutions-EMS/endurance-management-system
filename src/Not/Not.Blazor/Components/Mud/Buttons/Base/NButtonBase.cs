using MudBlazor;
using Not.Localization;

namespace Not.Blazor.Components;

public abstract class NButtonBase : MudButton
{
    [Inject]
    ILocalizer Localizer { get; set; } = default!;

    [Parameter]
    public string? Text { get; set; }

    protected override void OnParametersSet()
    {
        ChildContent =
            Text == null ? null : new RenderFragment(x => x.AddContent(0, Localizer.Get(Text)));
    }
}
