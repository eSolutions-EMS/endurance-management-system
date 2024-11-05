using MudBlazor;
using Not.Services;

namespace Not.Blazor.Mud.Components.Base;

// TODO: figure out if it is possible to internally localize the contents here
public abstract class NotHeadlineBase : MudText
{
    [Inject]
    protected ILocalizer Localizer { get; set; } = default!;

    protected NotHeadlineBase()
    {
        Align = Align.Center;
    }
}
