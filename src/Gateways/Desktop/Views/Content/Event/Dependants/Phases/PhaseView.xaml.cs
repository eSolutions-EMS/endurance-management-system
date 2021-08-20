using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Phases
{
    public partial class PhaseView : UserControl, IView
    {
        public PhaseView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
