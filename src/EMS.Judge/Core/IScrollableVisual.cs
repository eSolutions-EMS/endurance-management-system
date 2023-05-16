using System.Windows.Input;

namespace EMS.Judge.Core;

public interface IScrollableVisual
{
    void HandleScroll(object sender, MouseWheelEventArgs args);
}
