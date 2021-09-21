using EnduranceJudge.Core.ConventionalServices;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core.Static
{
    public interface IMouseInputService : IService
    {
        void HandleScroll(object sender, MouseWheelEventArgs scrollEvent);
    }
}
