using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public partial class EnduranceEventView : UserControl, IView
    {
        public EnduranceEventView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
