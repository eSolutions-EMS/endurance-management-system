using EMS.Judge.Common;
using EMS.Judge.Common.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.Judge.Views.Content.Configuration.Children.Competitions.AddParticipants;

public partial class AddParticipantsView : UserControl, IView
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
