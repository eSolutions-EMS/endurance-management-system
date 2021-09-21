using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations
{
    public partial class ParticipationView : UserControl, IView
    {
        public ParticipationView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.CONTENT_LEFT;
    }
}
