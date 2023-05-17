using EMS.Judge.Common;
using EMS.Judge.Common.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.Judge.Views.Content.Hardware;

public partial class HardwareView : UserControl, IView
{
    private readonly IInputHandler handler;
    public HardwareView(IInputHandler handler) : this()
    {
        this.handler = handler;
    }
    public HardwareView()
    {
        InitializeComponent();
    }

    public string RegionName => Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.handler.HandleScroll(sender, mouseEvent);
    }
}
