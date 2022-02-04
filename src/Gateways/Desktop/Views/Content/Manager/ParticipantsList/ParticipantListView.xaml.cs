using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.ParticipantsList
{
    public partial class ParticipantListView : UserControl, IScrollableView
    {
        private readonly IInputHandler inputInput;

        public ParticipantListView(IInputHandler inputInput) : this()
        {
            this.inputInput = inputInput;
        }
        public ParticipantListView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.CONTENT_RIGHT;

        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.inputInput.HandleScroll(sender, mouseEvent);
        }
    }
}
