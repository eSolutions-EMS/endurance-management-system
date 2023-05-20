using EMS.Judge.Common;
using EMS.Judge.Common.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.Judge.Views.Content.Manager.ParticipantsList;

public partial class ParticipantListView : UserControl, IView
{
    private readonly IInputHandler inputInput;

    public ParticipantListView(IInputHandler inputInput) : this()
    {
        this.inputInput = inputInput;
    }
    public ParticipantListView()
    {
        InitializeComponent();
    }

    public string RegionName { get; } = Regions.CONTENT_RIGHT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.inputInput.HandleScroll(sender, mouseEvent);
    }
}
