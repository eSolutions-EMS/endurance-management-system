using Core.Events;
using Microsoft.AspNetCore.Components;

namespace EMS.Witness.Common;

public abstract class StatefulComponent<T> : ComponentBase
    where T : class, new()
{
    public T State { get; set; } = new();

    public StatefulComponent()
    {
        AppState.Changed += OnStateChanged;
    }

    /// <summary>
    /// Compares the references of component's state and changedState param and calls StateHasChanged if equal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="changedState"></param>
    /// <returns></returns>
    protected virtual async void OnStateChanged(object? sender, object changedState)
    {
        if (changedState.Equals(this.State))
        {
            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}
