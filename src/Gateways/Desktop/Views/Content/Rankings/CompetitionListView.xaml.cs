using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings;

public partial class CompetitionListView : UserControl, IView
{
    private readonly IInputHandler inputHandler;
    public CompetitionListView(IInputHandler inputHandler) : this()
    {
        this.inputHandler = inputHandler;
        this.inputHandler = inputHandler;
    }
    public CompetitionListView()
    {
        InitializeComponent();
    }

    public string RegionName => Regions.CONTENT_RIGHT;

    public void HandleScroll(object sender, MouseWheelEventArgs args)
    {
        this.inputHandler.HandleScroll(sender, args);
    }
}