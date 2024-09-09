using MudBlazor;
using Not.Blazor.Mud.Components.Base;

namespace Not.Blazor.Mud.Components;

public class NotButtonPrimary : NotButtonBase
{
    public NotButtonPrimary()
    {
        Color = Color.Primary;
        Variant = Variant.Filled;
        IconSize = Size.Medium;
    }
}
