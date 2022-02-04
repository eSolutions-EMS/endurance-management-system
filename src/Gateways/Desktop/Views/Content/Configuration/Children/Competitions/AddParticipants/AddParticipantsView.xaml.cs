using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions.AddParticipants;

public partial class AddParticipantsView : UserControl, IScrollableView
{
    private readonly IInputHandler inputInput;

    public AddParticipantsView(IInputHandler inputInput) : this()
    {
        this.inputInput = inputInput;
    }
    public AddParticipantsView()
    {
        InitializeComponent();
    }

    public string RegionName => Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.inputInput.HandleScroll(sender, mouseEvent);
    }
}
