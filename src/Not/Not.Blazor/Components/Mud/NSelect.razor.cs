using MudBlazor;
using Not.Structures;

namespace Not.Blazor.Components;

public partial class NSelect<T>
{
    static NotListModel<T> _empty = NotListModel.Empty<T>();

    [Parameter]
    public List<NotListModel<T>> Items { get; set; } = [];

    [Parameter]
    public string Label { get; set; } = default!;

    [Parameter]
    public string Placeholder { get; set; } = default!;

    public MudBaseInput<T?> MudBaseInput { get; private set; } = default!;
}
