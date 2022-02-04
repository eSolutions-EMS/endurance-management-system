using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Phases
{
    public partial class PhaseView : UserControl, IView
    {
        private readonly IInputHandler inputInput;

        public PhaseView()
        {
            InitializeComponent();
        }

        public PhaseView(IInputHandler inputInput) : this()
        {
            this.inputInput = inputInput;
        }

        public string RegionName { get; } = Regions.CONTENT_LEFT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.inputInput.HandleScroll(sender, mouseEvent);
        }
    }
}
