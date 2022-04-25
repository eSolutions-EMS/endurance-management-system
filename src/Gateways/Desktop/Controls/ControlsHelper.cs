using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Controls;

public class ControlsHelper
{
    public static Style GetStyle(string key)
        => (Style) System.Windows.Application.Current.FindResource(key);
}
