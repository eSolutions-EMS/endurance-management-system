using Endurance.Gateways.Witness.Services;
using Microsoft.AspNetCore.Components;

namespace Endurance.Gateways.Witness.Shared.Toasts;

public partial class Toaster : ComponentBase, IDisposable
{
    [Inject] 
    private ToasterService ToasterService { get; set; }

    protected override void OnInitialized()
    {
        this.ToasterService.ToasterChanged += HandleToastChanged;
        this.ToasterService.ToasterTimerElapsed += HandleToastChanged;
    }

    public void Dispose()
    {
        this.ToasterService.ToasterChanged -= HandleToastChanged;
        this.ToasterService.ToasterTimerElapsed -= HandleToastChanged;
    }

    private void HandleToastChanged(object? sender, EventArgs e)
        => this.InvokeAsync(this.StateHasChanged);

    private string GetToastClass(Toast toast)
    {
        var colour = Enum.GetName(toast.Color)?.ToLower();
        return $"bg-{colour} text-white";
    }
}
