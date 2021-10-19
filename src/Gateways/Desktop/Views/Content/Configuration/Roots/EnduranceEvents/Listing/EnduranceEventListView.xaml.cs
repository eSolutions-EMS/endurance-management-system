using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Roots.EnduranceEvents.Listing
{
    public partial class EnduranceEventListView : UserControl, IView
    {
        public EnduranceEventListView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.CONTENT_LEFT;
    }
}
