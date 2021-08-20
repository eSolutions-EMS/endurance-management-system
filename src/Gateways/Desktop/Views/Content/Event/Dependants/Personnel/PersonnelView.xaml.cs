using EnduranceJudge.Gateways.Desktop.Core;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Personnel
{
    public partial class PersonnelView : UserControl, IView
    {
        public PersonnelView()
        {
            InitializeComponent();
        }

        public string RegionName { get; } = Regions.Content;
    }
}
