using System.Windows.Controls;
using System.Windows.Input;
using EMS.Judge.Common;
using EMS.Judge.Common.Services;

namespace EMS.Judge.Views.Navigation;

public partial class NavigationView : UserControl, IView
{
    private readonly IInputHandler inputHandler;

    public NavigationView(IInputHandler inputHandler)
        : this()
    {
        this.inputHandler = inputHandler;
        this.inputHandler = inputHandler;
    }

    public NavigationView()
    {
        InitializeComponent();
    }

    public string RegionName { get; } = Regions.HEADER_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs args)
    {
        this.inputHandler.HandleScroll(sender, args);
    }
}
