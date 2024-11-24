using MudBlazor;

namespace Not.Blazor.Components;

public class NButtonDelete : NButtonPrimary
{
    public NButtonDelete()
    {
        StartIcon = Icons.Material.Filled.Delete;
        Text = "Delete";
    }
}
