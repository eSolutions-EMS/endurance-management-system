using Microsoft.AspNetCore.Components;
using WitnessState = EMS.Witness.Services.State;

namespace EMS.Witness.Common;

public abstract class StatefulComponent<T> : ComponentBase
    where T : class
{
    [Parameter]
    public abstract T State { get; set; }

    public StatefulComponent()
    {
        WitnessState.StateChanged += OnStateChanged;
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
