using MudBlazor;

namespace Not.Blazor.Mud.Extensions
{
    public static class SnackbarConfigurationExtensions
    {
        public static void SetVisibleDuration(this CommonSnackbarOptions options, TimeSpan duration)
        {
            options.VisibleStateDuration = (int)duration.TotalMilliseconds;
        }
    }
}
