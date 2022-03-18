using EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.ParticipantResults;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class RanklistPrinter : PrintTemplate
{
    public RanklistPrinter(string competitionName, IEnumerable<ParticipantResultTemplateModel> participations)
        : base(competitionName)
    {
        foreach (var participation in participations)
        {
            var control = new ContentControl { Content = participation };
            this.AddPrintContent(control);
        }
    }

    public override UIElement GetTable(out double reserveHeightOf, out Brush borderBrush)
    {
        reserveHeightOf = this.HeaderOffset;
        borderBrush = this.BorderBrush;
        return new Border();
    }
}
