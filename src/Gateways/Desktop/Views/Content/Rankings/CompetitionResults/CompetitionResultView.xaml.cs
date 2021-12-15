using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.CompetitionResults
{
    public partial class CompetitionResultView : UserControl, IScrollableView
    {
        private readonly IMouseInputService inputService;
        public CompetitionResultView(IMouseInputService inputService) : this()
        {
            this.inputService = inputService;
        }
        public CompetitionResultView()
        {
            InitializeComponent();
        }

        public string RegionName => Regions.CONTENT_LEFT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.inputService.HandleScroll(sender, mouseEvent);
        }
    }
}
