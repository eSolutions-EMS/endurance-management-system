using EMS.Judge.Core;
using EMS.Judge.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.Judge.Views.Content.Configuration.Roots.Athletes.Listing;

public partial class AthleteScrollableView : UserControl, IView
{
    private readonly IInputHandler inputInput;

    public AthleteScrollableView()
    {
        InitializeComponent();
    }

    public AthleteScrollableView(IInputHandler inputInput) : this()
    {
        this.inputInput = inputInput;
    }

    public string RegionName { get; } = Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.inputInput.HandleScroll(sender, mouseEvent);
    }
}
