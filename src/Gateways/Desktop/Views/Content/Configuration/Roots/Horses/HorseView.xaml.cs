using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Horses;

public partial class HorseView : UserControl, IView
{
    private readonly IInputHandler inputHandler;
    public HorseView(IInputHandler inputHandler) : this()
    {
        this.inputHandler = inputHandler;
        this.inputHandler = inputHandler;
    }
    public HorseView()
    {
        InitializeComponent();
    }

    public string RegionName { get; } = Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs args)
    {
        this.inputHandler.HandleScroll(sender, args);
    }
}