using System.Windows.Controls;
using System.Windows.Input;
using EMS.Judge.Common;
using EMS.Judge.Common.Services;

namespace EMS.Judge.Views.Content.Configuration.Children.Competitions;

public partial class CompetitionView : UserControl, IView
{
    private readonly IInputHandler inputInput;

    public CompetitionView()
    {
        InitializeComponent();
    }

    public CompetitionView(IInputHandler inputInput)
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
