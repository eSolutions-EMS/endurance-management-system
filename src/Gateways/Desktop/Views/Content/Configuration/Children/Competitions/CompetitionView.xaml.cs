using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Competitions
{
    public partial class CompetitionView : UserControl, IScrollableView
    {
        private readonly IMouseHandler mouseInput;

        public CompetitionView()
        {
            InitializeComponent();
        }

        public CompetitionView(IMouseHandler mouseInput) : this()
        {
            this.mouseInput = mouseInput;
        }

        public string RegionName { get; } = Regions.CONTENT_LEFT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.mouseInput.HandleScroll(sender, mouseEvent);
        }
    }
}
