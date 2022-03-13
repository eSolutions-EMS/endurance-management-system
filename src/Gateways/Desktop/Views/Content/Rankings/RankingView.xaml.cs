using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.CompetitionResults;

public partial class RankingView : UserControl, IView
{
    private readonly IInputHandler handler;
    public RankingView(IInputHandler handler) : this()
    {
        this.handler = handler;
    }
    public RankingView()
    {
        InitializeComponent();
    }

    public string RegionName => Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.handler.HandleScroll(sender, mouseEvent);
    }
}
