using EnduranceJudge.Core.ConventionalServices;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core.Services
{
    public interface IMouseHandler : IService
    {
        void HandleScroll(object sender, MouseWheelEventArgs scrollEvent);
    }
}
