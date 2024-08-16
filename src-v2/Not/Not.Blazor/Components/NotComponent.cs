using Not.Blazor.Ports.Behinds;
using Not.Services;

namespace Not.Blazor.Components;

public class NotComponent : ComponentBase
{
    [Inject]
    protected ILocalizer Localizer { get; set; } = default!;

    protected void Observe(IObservableBehind observable)
    {
        observable.Subscribe(Render);
    }

    protected async Task Render()
    {
        await InvokeAsync(StateHasChanged);
    }
}
