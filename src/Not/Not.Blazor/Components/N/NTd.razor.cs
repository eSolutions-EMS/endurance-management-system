namespace Not.Blazor.Components;

public partial class NTd<T>
{
    [Parameter]
    public T? Value { get; set; }
}
