using EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.ParticipantResults;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class RanklistPrinter : PrintTemplate
{
    public RanklistPrinter(string competitionName, IEnumerable<ParticipationResultTemplateModel> participations)
        : base(competitionName)
    {
        foreach (var participation in participations)
        {
            var control = new ContentControl { Content = participation };
            this.AddPrintContent(control);
        }
    }
}
