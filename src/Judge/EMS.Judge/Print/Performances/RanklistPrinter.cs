using EMS.Judge.Controls.Ranking;
using Core.Domain.AggregateRoots.Ranking.Aggregates;

namespace EMS.Judge.Print.Performances;

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
