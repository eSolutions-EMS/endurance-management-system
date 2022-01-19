using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.ParticipantsList
{
    public partial class ParticipantListView : UserControl, IScrollableView
    {
        private readonly IMouseHandler mouseInput;

        public ParticipantListView(IMouseHandler mouseInput) : this()
        {
            this.mouseInput = mouseInput;
        }
        public ParticipantListView()
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
