using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Athletes.Listing
{
    public partial class AthleteScrollableView : UserControl, IScrollableView
    {
        private readonly IMouseHandler mouseInput;

        public AthleteScrollableView()
        {
            InitializeComponent();
        }

        public AthleteScrollableView(IMouseHandler mouseInput) : this()
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
