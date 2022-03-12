using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Laps
{
    public partial class LapView : UserControl, IView
    {
        private readonly IInputHandler inputInput;

        public LapView()
        {
            InitializeComponent();
        }

        public LapView(IInputHandler inputInput) : this()
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
