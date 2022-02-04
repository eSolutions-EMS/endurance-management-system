using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.Athletes
{
    public partial class AthleteView : UserControl, IView
    {
        private readonly IInputHandler inputHandler;
        public AthleteView(IInputHandler inputHandler) : this()
        {
            this.inputHandler = inputHandler;
            this.inputHandler = inputHandler;
        }
        public AthleteView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.CONTENT_LEFT;

        public void HandleScroll(object sender, MouseWheelEventArgs args)
        {
            this.inputHandler.HandleScroll(sender, args);
        }
    }
}
