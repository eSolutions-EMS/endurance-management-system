using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions.AddParticipants;

public partial class AddParticipantsView : UserControl, IScrollableView
{
    private readonly IMouseInputService mouseInput;

    public AddParticipantsView(IMouseInputService mouseInput) : this()
    {
        this.mouseInput = mouseInput;
    }
    public AddParticipantsView()
    {
        InitializeComponent();
    }

    public string RegionName => Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.mouseInput.HandleScroll(sender, mouseEvent);
    }
}
