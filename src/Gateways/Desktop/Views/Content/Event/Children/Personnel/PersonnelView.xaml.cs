using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Personnel
{
    public partial class PersonnelView : UserControl, IScrollableView
    {
        private readonly IMouseInputService mouseInput;

        public PersonnelView()
        {
            InitializeComponent();
        }

        public PersonnelView(IMouseInputService mouseInput) : this()
        {
            this.mouseInput = mouseInput;
        }

        public string RegionName { get; } = Regions.CONTENT_RIGHT;
        public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
        {
            this.mouseInput.HandleScroll(sender, mouseEvent);
        }
    }
}
