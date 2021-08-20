using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.PhasesForCategory
{
    public partial class PhaseForCategoryView : UserControl, IView
    {
        public PhaseForCategoryView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
