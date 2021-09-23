using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings
{
    public partial class RankingView : UserControl, IView
    {
        public RankingView()
        {
            InitializeComponent();
        }
        public string RegionName { get; } = Regions.CONTENT_LEFT;
    }
}
