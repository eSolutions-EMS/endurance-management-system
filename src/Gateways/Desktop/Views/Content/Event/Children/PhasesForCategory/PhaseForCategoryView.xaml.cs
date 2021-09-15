using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.PhasesForCategory
{
    public partial class PhaseForCategoryView : UserControl, IScrollableView
    {
        private readonly IMouseInputService mouseInput;

        public PhaseForCategoryView()
        {
            InitializeComponent();
        }

        public PhaseForCategoryView(IMouseInputService mouseInput) : this()
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
