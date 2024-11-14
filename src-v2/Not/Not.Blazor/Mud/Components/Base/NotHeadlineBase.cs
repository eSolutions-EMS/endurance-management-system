using MudBlazor;
using Not.Localization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Not.Blazor.Components.Base;
#pragma warning restore IDE0130 // Namespace does not match folder structure

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
