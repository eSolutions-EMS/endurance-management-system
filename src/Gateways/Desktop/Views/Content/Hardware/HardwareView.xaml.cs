using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Hardware;

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
