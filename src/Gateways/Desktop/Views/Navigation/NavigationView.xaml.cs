using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Navigation
{
    public partial class NavigationView : UserControl, IView
    {
        public NavigationView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.HEADER_LEFT;
    }
}
