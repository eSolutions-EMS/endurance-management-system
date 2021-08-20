using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Participants
{
    public partial class ParticipantView : UserControl, IView
    {
        public ParticipantView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
