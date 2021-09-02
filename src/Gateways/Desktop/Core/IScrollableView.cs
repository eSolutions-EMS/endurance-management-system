using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core
{
    public interface IScrollableView : IView
    {
        void HandleScroll(object sender, MouseWheelEventArgs mouseEvent);
    }
}
