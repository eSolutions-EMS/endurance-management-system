using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Import
{
    public partial class ImportView : UserControl, IView
    {
        public string RegionName { get; } = Regions.Content;

        public ImportView()
        {
            InitializeComponent();
        }
    }
}
