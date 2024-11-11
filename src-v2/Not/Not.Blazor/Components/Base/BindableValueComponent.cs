namespace Not.Blazor.Components.Base;

public class BindableValueComponent<T> : NotComponent
{
    T? _value;

    [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
    public T? Value
#pragma warning restore BL0007 
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
