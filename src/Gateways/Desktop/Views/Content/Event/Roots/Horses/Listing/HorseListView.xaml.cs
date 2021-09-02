using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Horses.Listing
{
    public partial class HorseListView : UserControl, IView
    {
        private readonly IMouseInputService mouseInput;

        public HorseListView()
        {
        }

        public HorseListView(IMouseInputService mouseInput)
        {
            this.mouseInput = mouseInput;
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;

        private void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.mouseInput.HandleScroll(sender, mouseEvent);
        }
    }
}
