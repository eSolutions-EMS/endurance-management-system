using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;

namespace EnduranceJudge.Gateways.Desktop.Controls.Ranking;

public partial class RanklistControl
{
    public RanklistControl(RankList rankList)
    {
        for (var rank = 1; rank <= rankList.Count; rank++)
        {
            var participation = rankList[rank];
            var result = new ParticipationResultModel(rank, participation);
            var control = new ParticipationResultControl(result);
            this.Children.Add(control);
        }
    }
    public RanklistControl()
    {
        InitializeComponent();
    }
}
