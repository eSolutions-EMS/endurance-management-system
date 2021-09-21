using EnduranceJudge.Gateways.Desktop.Core.Static;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core.Services.Implementations
{
    public class MouseInputService : IMouseInputService
    {
        public void HandleScroll(object sender, MouseWheelEventArgs scrollEvent)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - scrollEvent.Delta);
            scrollEvent.Handled = true;
        }
    }
}
