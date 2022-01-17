using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.EnduranceEvents
{
    public partial class EnduranceEventView : UserControl, IScrollableView
    {
        private readonly IMouseHandler mouseInput;

        public EnduranceEventView()
        {
            InitializeComponent();
        }

        public EnduranceEventView(IMouseHandler mouseInput) : this()
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
