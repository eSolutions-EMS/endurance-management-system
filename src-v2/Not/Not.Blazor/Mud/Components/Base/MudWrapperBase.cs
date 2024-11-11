using Microsoft.AspNetCore.Components;
using Not.Blazor.Components.Base;

namespace Not.Blazor.Mud.Components.Base;

public class MudWrapperBase : NotComponent
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
