using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Horses
{
    public partial class HorseView : UserControl, IView
    {
        public HorseView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
