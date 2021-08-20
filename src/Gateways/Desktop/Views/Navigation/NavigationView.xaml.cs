using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Navigation
{
    public partial class NavigationView : UserControl, IView
    {
        public string RegionName { get; } = Regions.Navigation;

        public NavigationView()
        {
            InitializeComponent();
        }

    }
}
