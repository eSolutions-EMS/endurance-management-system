using MudBlazor;
using Not.Blazor.Mud.Extensions;
using Not.Notifier;

namespace Not.Blazor.Notifier;

public class BlazorNotifier : ComponentBase
{
    readonly TimeSpan _failedDuration = TimeSpan.FromSeconds(30);

    public BlazorNotifier()
    {
        Notifications.InformedEvent.SubscribeAsync(AddSnack);
        Notifications.SucceededEvent.SubscribeAsync(AddSnack);
        Notifications.WarnedEvent.SubscribeAsync(AddSnack);
        Notifications.FailedEvent.SubscribeAsync(AddSnack);
    }

    [Inject]
    ISnackbar Snackbar { get; set; } = default!;

    void AddSnack(Informed informed)
    {
        Snackbar.Add(informed.Message, Severity.Info);
    }

    void AddSnack(Warned warned)
    {
        Snackbar.Add(warned.Message, Severity.Warning);
    }

    void AddSnack(Failed failed)
    {
        Snackbar.Add(
            failed.Message,
            Severity.Error,
            config => config.SetVisibleDuration(_failedDuration)
        );
    }

    void AddSnack(Succeeded succeeded)
    {
        Snackbar.Add(succeeded.Message, Severity.Success);
    }
}
