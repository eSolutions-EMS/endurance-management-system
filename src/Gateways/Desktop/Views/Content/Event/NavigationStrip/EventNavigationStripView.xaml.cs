using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.NavigationStrip
{
    public partial class EventNavigationStripView : UserControl, IView
    {
        public EventNavigationStripView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.SubNavigation;
    }
}
