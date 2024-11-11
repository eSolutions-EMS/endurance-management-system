using System.Windows.Controls;
using System.Windows.Input;
using EMS.Judge.Common;
using EMS.Judge.Common.Services;

namespace EMS.Judge.Views.Content.Configuration.ConfigurationMenu;

public partial class ConfigurationMenuView : UserControl, IView
{
    private readonly IInputHandler inputHandler;

    public ConfigurationMenuView(IInputHandler inputHandler)
        : this()
    {
        this.inputHandler = inputHandler;
    }

    public ConfigurationMenuView()
    {
        InitializeComponent();
    }

    public string RegionName { get; } = Regions.CONTENT_RIGHT;

    public void HandleScroll(object sender, MouseWheelEventArgs args)
    {
        this.inputHandler.HandleScroll(sender, args);
    }
}
