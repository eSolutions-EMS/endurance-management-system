using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.ConfigurationMenu
{
    public partial class ConfigurationMenuView : UserControl, IView
    {
        private readonly IInputHandler inputHandler;
        public ConfigurationMenuView(IInputHandler inputHandler) : this()
        {
            this.inputHandler = inputHandler;
        }
        public ConfigurationMenuView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.CONTENT_RIGHT;
        public void HandleScroll(object sender, MouseWheelEventArgs args)
        {
            this.inputHandler.HandleScroll(sender, args);
        }
    }
}
