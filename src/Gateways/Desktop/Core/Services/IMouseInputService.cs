using EnduranceJudge.Core.ConventionalServices;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core.Services
{
    public interface IMouseInputService : IService
    {
        void HandleScroll(object sender, MouseWheelEventArgs scrollEvent);
    }
}
