using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Error
{
    public partial class ErrorView : UserControl, IView
    {
        public ErrorView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.CONTENT_LEFT;
    }
}
