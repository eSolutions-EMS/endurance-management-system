using EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class PrintPerformance : PrintTemplate
{
    public PrintPerformance(ParticipationTemplateModel participation) : base(participation.Number.ToString())
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
