using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.PrintExample
{
    public partial class PrintExampleView : UserControl, IView
    {
        public PrintExampleView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
