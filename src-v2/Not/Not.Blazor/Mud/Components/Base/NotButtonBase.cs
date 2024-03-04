using MudBlazor;
using Not.Services;

namespace Not.Blazor.Mud.Components.Base;

public class NotButtonBase : MudButton
{
    [Inject]
    private ILocalizer _localizer { get; set; } = default!;

    protected void UseText(string text)
    {
        ChildContent = new RenderFragment(x => x.AddContent(0, _localizer.Get(text)));
    }
}
