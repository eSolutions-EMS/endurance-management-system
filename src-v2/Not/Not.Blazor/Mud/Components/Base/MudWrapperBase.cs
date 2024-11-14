using Not.Blazor.Components;

namespace Not.Blazor.Mud.Components.Base;

public class MudWrapperBase : NComponent
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
