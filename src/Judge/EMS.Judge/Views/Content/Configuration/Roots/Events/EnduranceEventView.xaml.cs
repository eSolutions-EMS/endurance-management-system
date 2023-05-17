using EMS.Judge.Common;
using EMS.Judge.Common.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.Judge.Views.Content.Configuration.Roots.Events;

public partial class EnduranceEventView : UserControl, IView
{
    private readonly IInputHandler inputInput;

    public EnduranceEventView()
    {
        InitializeComponent();
    }

    public EnduranceEventView(IInputHandler inputInput) : this()
    {
        this.inputInput = inputInput;
    }

    public string RegionName { get; } = Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.inputInput.HandleScroll(sender, mouseEvent);
    }
}
