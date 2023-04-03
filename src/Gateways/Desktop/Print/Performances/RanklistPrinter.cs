using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;
using EnduranceJudge.Gateways.Desktop.Controls.Ranking;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class RanklistPrinter : PrintTemplate
{
    public RanklistPrinter(string competitionName, RanklistAggregate rankList)
        : base(competitionName)
    {
        var models = RanklistControl.CreateResultControls(rankList);

        foreach (var model in models)
        {
            var control = new ParticipationResultControl() { Participation = model };
            this.AddPrintContent(control);
        }
    }
}
