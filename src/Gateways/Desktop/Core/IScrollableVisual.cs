using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core;

public interface IScrollableVisual
{
    void HandleScroll(object sender, MouseWheelEventArgs args);
}
