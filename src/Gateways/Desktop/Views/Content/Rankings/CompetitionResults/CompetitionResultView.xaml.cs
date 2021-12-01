using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.Categorizations
{
    public partial class CompetitionResultView : UserControl, IView
    {
        public CompetitionResultView()
        {
            InitializeComponent();
        }
        public string RegionName { get; } = Regions.CONTENT_LEFT;
    }
}
