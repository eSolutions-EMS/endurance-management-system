using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations.Listing
{
    public partial class ParticipationListView : UserControl, IScrollableView
    {
        private readonly IMouseInputService mouseInput;

        public ParticipationListView(IMouseInputService mouseInput) : this()
        {
            this.mouseInput = mouseInput;
        }
        public ParticipationListView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.CONTENT_RIGHT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.mouseInput.HandleScroll(sender, mouseEvent);
        }
    }
}
