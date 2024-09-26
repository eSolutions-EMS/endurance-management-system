using MudBlazor;
using Not.Events;
using Not.Notifier;

namespace Not.Blazor.Notifier;

public class BlazorNotifier : ComponentBase
{
    [Inject]
    ISnackbar _snackbar { get; set; } = default!;

    public BlazorNotifier()
    {
        EventHelper.Subscribe<Informed>(AddSnack);
        EventHelper.Subscribe<Warned>(AddSnack);
        EventHelper.Subscribe<Failed>(AddSnack);
        EventHelper.Subscribe<Succeeded>(AddSnack);
    }

    void AddSnack(Informed informed)
    {
        _snackbar.Add(informed.Message, Severity.Info);
    }

    void AddSnack(Warned warned)
    {
        _snackbar.Add(warned.Message, Severity.Warning);
    }

    void AddSnack(Failed failed)
    {
        _snackbar.Add(failed.Message, Severity.Error);
    }

    void AddSnack(Succeeded succeeded)
    {
        _snackbar.Add(succeeded.Message, Severity.Success);
    }
}
