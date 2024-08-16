using MudBlazor;

namespace Not.Blazor.TM.Forms;

public interface IMudBaseInputWrapper<T>
{
    MudBaseInput<T> MudBaseInput { get; }
}
