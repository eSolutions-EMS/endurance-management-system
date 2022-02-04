using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core
{
    public interface IView
    {
        string RegionName { get; }
        void HandleScroll(object sender, MouseWheelEventArgs args);
        void HandleKeyboardFocus(object sender, KeyboardFocusChangedEventArgs args);
    }
}
