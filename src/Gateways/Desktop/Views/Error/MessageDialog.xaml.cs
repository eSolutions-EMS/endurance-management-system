using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Error
{
    public partial class MessageDialog : UserControl, IView
    {
        public MessageDialog()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.CONTENT_LEFT;
    }
}
