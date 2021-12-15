using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager
{
    public partial class ContestManagerView : UserControl, IScrollableView
    {
        private readonly IMouseInputService mouseInput;

        public ContestManagerView(IMouseInputService mouseInput) : this()
        {
            this.mouseInput = mouseInput;
        }
        public ContestManagerView()
        {
            InitializeComponent();
        }

        public string RegionName => Regions.CONTENT_LEFT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.mouseInput.HandleScroll(sender, mouseEvent);
        }
    }
}
