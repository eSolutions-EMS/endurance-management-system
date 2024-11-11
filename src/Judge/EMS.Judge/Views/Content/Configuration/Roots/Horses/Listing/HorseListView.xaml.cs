using System.Windows.Controls;
using System.Windows.Input;
using EMS.Judge.Common;
using EMS.Judge.Common.Services;

namespace EMS.Judge.Views.Content.Configuration.Roots.Horses.Listing;

public partial class HorseListView : UserControl, IView
{
    private readonly IInputHandler inputInput;

    public HorseListView()
    {
        InitializeComponent();
    }

    public HorseListView(IInputHandler inputInput)
        : this()
    {
        this.inputInput = inputInput;
    }

    public string RegionName { get; } = Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.inputInput.HandleScroll(sender, mouseEvent);
    }
}
