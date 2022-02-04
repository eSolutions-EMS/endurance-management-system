using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager
{
    public partial class ContestManagerView : UserControl, IScrollableView
    {
        private readonly IInputHandler inputInput;

        public ContestManagerView(IInputHandler inputInput) : this()
        {
            this.inputInput = inputInput;
        }
        public ContestManagerView()
        {
            InitializeComponent();
        }

        public string RegionName => Regions.CONTENT_LEFT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.inputInput.HandleScroll(sender, mouseEvent);
        }
    }
}
