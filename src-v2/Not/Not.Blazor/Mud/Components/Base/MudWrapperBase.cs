using Not.Blazor.Components;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Not.Blazor.Components.Base;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class MudWrapperBase : NComponent
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
