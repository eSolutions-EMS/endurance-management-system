using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes
{
    public partial class AthleteView : UserControl, IView
    {
        public AthleteView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
