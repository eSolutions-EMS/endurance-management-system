using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class PerfomancePrinter : PrintTemplate
{
    // TODO: add error handling
    public PerfomancePrinter(ParticipationGridModel participation) : base(participation.Number.ToString())
    {
        var control = new ContentControl { Content = participation };
        this.AddPrintContent(control);
    }

    public override UIElement GetTable(out double reserveHeightOf, out Brush borderBrush)
    {
        reserveHeightOf = this.HeaderOffset;
        borderBrush = this.BorderBrush;
        return new Border();
    }
}
