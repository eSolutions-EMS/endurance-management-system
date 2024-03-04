using Microsoft.AspNetCore.Components;
using Not.Blazor.Components;

namespace Not.Blazor.Mud.Components.Base;

public class MudWrapperBase : NotComponent
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
