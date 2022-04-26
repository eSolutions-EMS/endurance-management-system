using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;
using EnduranceJudge.Gateways.Desktop.Controls;
using EnduranceJudge.Gateways.Desktop.Controls.Ranking;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class RanklistPrinter : PrintTemplate
{
    public RanklistPrinter(string competitionName, RankList rankList)
        : base(competitionName)
    {
        foreach (var participation in rankList)
        {
            var model = new ParticipationResultModel(1, participation);
            var control = new ParticipationResultControl(model);
            control.Measure(this.PrintDimension.PageSize);
            control.Arrange(new Rect());
            control.Scale(0.75);
            this.AddPrintContent(control);
        }
    }
}
