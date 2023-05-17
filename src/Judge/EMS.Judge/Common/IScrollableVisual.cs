using System.Windows.Input;

namespace EMS.Judge.Common;

public interface IScrollableVisual
{
    void HandleScroll(object sender, MouseWheelEventArgs args);
}
