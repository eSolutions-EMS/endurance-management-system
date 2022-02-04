using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core.Services.Implementations
{
    public class InputHandler : IInputHandler
    {
        public void HandleScroll(object sender, MouseWheelEventArgs scrollEvent)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - scrollEvent.Delta);
            scrollEvent.Handled = true;
        }
    }
}
