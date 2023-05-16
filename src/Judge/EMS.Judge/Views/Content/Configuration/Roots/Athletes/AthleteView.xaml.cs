using EMS.Judge.Core;
using EMS.Judge.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.Judge.Views.Content.Configuration.Roots.Athletes;

public partial class AthleteView : UserControl, IView
{
    private readonly IInputHandler inputHandler;
    public AthleteView(IInputHandler inputHandler) : this()
    {
        this.inputHandler = inputHandler;
        this.inputHandler = inputHandler;
    }
    public AthleteView()
    {
        InitializeComponent();
    }

    public string RegionName { get; } = Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs args)
    {
        this.inputHandler.HandleScroll(sender, args);
    }
}
