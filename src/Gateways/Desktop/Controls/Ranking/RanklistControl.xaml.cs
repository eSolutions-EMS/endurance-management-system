using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;
using System.Collections.Generic;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Controls.Ranking;

public partial class RanklistControl
{
    public RanklistControl()
    {
        this.InitializeComponent();
    }

    public RanklistAggregate Ranklist
    {
        get => (RanklistAggregate)this.GetValue(RANKLIST_PROPERTY);
        set => this.SetValue(RANKLIST_PROPERTY, value);
    }

    public static readonly DependencyProperty RANKLIST_PROPERTY =
        DependencyProperty.Register(
            nameof(Ranklist),
            typeof(RanklistAggregate),
            typeof(RanklistControl),
            new PropertyMetadata(OnRanklistChanged));

    private static void OnRanklistChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        var control = (RanklistControl)sender;
        var participation = (RanklistAggregate)args.NewValue;
        control.Populate(participation);
    }

    private void Populate(RanklistAggregate rankList)
    {
        this.ItemsSource = CreateResultControls(rankList);
    }

    public static IEnumerable<ParticipationResultModel> CreateResultControls(RanklistAggregate rankList)
    {
        var list = new List<ParticipationResultModel>();
        for (var i = 0; i < rankList.Count; i++)
        {
            var rank = i + 1;
            var participation = rankList[i];
            var result = new ParticipationResultModel(rank, participation);
            list.Add(result);
        }
        return list;
    }
}
