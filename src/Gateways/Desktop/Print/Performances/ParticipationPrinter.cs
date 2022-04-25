using EnduranceJudge.Gateways.Desktop.Controls;
using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class ParticipationPrinter : PrintTemplate
{
    public ParticipationPrinter(ParticipationGridModel participation) : base(participation.Number.ToString())
    {
        var control = new ParticipationGridControl(participation, true);
        control.Arrange(new Rect());
        control.Scale(0.75);
        this.AddPrintContent(control);
    }

    public override UIElement GetTable(out double reserveHeightOf, out Brush borderBrush)
    {
        reserveHeightOf = this.HeaderOffset;
        borderBrush = this.BorderBrush;
        return new Border();
    }
}
