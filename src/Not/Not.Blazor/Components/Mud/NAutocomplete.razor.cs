using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Not.Blazor.Components;

public partial class NAutocomplete<T>
{
    [Parameter, EditorRequired]
    public Func<string, Task<IEnumerable<T>>> Search { get; set; } = default!;
    [Parameter]
    public bool ResetValueOnClick { get; set; } = true;
    [Parameter]
    public string Label { get; set; } = "";

    public MudBaseInput<T> MudBaseInput { get; private set; } = default!;

    void HandleOnClick(MouseEventArgs _)
    {
        if (ResetValueOnClick)
        {
            Value = default;
        }
    }
}
