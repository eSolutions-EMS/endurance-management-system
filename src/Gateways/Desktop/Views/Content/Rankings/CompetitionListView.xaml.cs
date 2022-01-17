using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings
{
    public partial class CompetitionListView : UserControl, IView
    {
        public CompetitionListView()
        {
            InitializeComponent();
        }

        public string RegionName => Regions.CONTENT_RIGHT;
    }
}
