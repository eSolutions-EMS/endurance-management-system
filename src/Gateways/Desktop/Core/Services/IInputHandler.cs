using EnduranceJudge.Core.ConventionalServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core.Services;

public class InputHandler : IInputHandler
{
    public void HandleScroll(object sender, MouseWheelEventArgs scrollEvent)
    {
        var scv = (ScrollViewer)sender;
        scv.ScrollToVerticalOffset(scv.VerticalOffset - scrollEvent.Delta);
        scrollEvent.Handled = true;
    }
}

public interface IInputHandler : IService
{
    void HandleScroll(object sender, MouseWheelEventArgs scrollEvent);
}
