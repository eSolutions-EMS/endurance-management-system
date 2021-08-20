using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Competitions
{
    public partial class CompetitionView : UserControl, IView
    {
        public CompetitionView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
