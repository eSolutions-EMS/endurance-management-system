using MudBlazor;
using Not.Localization;

namespace Not.Blazor.Mud.Components.Base;

// TODO: figure out if it is possible to internally localize the contents here
public abstract class NotHeadlineBase : MudText
{
    protected NotHeadlineBase()
    {
        Align = Align.Center;
    }

    [Inject]
    protected ILocalizer Localizer { get; set; } = default!;
}
