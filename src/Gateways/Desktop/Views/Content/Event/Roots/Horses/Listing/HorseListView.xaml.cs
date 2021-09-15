using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Horses.Listing
{
    public partial class HorseScrollableView : UserControl, IScrollableView
    {
        private readonly IMouseInputService mouseInput;

        public HorseScrollableView()
        {
            InitializeComponent();
        }

        public HorseScrollableView(IMouseInputService mouseInput) : this()
        {
            this.mouseInput = mouseInput;
        }

        public string RegionName { get; } = Regions.CONTENT_RIGHT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.mouseInput.HandleScroll(sender, mouseEvent);
        }
    }
}
