using Not.Blazor.Ports.Behinds;
using Not.Services;

namespace Not.Blazor.Components;

public class NotComponent : ComponentBase
{
    [Inject]
    protected ILocalizer Localizer { get; set; } = default!;

    protected async Task Observe(IObservableBehind observable)
    {
        observable.Subscribe(Render);

        await observable.Initialize();
        await Render();
    }

    protected async Task Render()
    {
        await InvokeAsync(StateHasChanged);
    }
}
