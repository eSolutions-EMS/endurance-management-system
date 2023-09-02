using EMS.Judge.Controls.Ranking;
using Core.Domain.AggregateRoots.Ranking.Aggregates;
using Core.Domain.Enums;

namespace EMS.Judge.Print.Performances;

public class RanklistPrinter : PrintTemplate
{
    public RanklistPrinter(string competitionName, RanklistAggregate rankList, string category)
        : base(competitionName, category)
    {
        var models = RanklistControl.CreateResultControls(rankList);

        foreach (var model in models)
        {
            var control = new ParticipationResultControl() { Participation = model };
            this.AddPrintContent(control);
        }
    }
}
