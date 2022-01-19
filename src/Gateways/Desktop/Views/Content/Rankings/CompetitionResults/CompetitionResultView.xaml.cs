using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.CompetitionResults
{
    public partial class CompetitionResultView : UserControl, IScrollableView
    {
        private readonly IMouseHandler handler;
        public CompetitionResultView(IMouseHandler handler) : this()
        {
            this.handler = handler;
        }
        public CompetitionResultView()
        {
            InitializeComponent();
        }

        public string RegionName => Regions.CONTENT_LEFT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.handler.HandleScroll(sender, mouseEvent);
        }
    }
}
