namespace Not.Blazor.Components;

public class BindableValueComponent<T> : NotComponent
{
    private T? _value;

    [Parameter]
    public T? Value
    {
        get => _value;
        set
        {
            if (_value?.GetHashCode() == value?.GetHashCode())
            {
                return;
            }
            _value = value;
            ValueChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<T> ValueChanged { get; set; }
}
