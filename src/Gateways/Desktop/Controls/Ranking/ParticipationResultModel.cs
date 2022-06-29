using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.LapRecords;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using EnduranceJudge.Gateways.Desktop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Controls.Ranking;

public class ParticipationResultModel : ParticipationGridModel
{
    public ParticipationResultModel(int rank, int columns, Participation participation)
        : base(participation, true, columns)
    {
        this.Rank = rank;
        this.HorseGenderString = participation.Participant.Horse.IsStallion ? STALLION : MARE; // TODO: move in domain

        // TODO: move in domain
        var (averageSpeed, time) = this.CalculateTotalValues(participation);
        this.TotalAverageSpeed = ValueSerializer.FormatDouble(averageSpeed);
        this.TotalTime = ValueSerializer.FormatSpan(time);

        var (notQualified, visibility) = this.HandleNotQualified(participation.Participant.LapRecords.ToList());
        this.DisqualifiedVisibility = visibility;
        this.NotQualifiedText = notQualified;
    }

    private (double? averageSpeed, TimeSpan time) CalculateTotalValues(Participation participation)
    {
        var performances = Performance.GetAll(participation).ToList();
        var totalAverageSpeed = performances.Sum(perf => perf.AverageSpeed) / performances.Count;
        var totalTime = performances
            .Where(x => x.Time.HasValue)
            .Aggregate(TimeSpan.Zero, (ag, perf) => ag + perf.Time!.Value);

        return (totalAverageSpeed, totalTime);
    }

    private (string notQualified, Visibility) HandleNotQualified(List<LapRecord> records)
    {
        var disqualifiedRecord = records.FirstOrDefault(x => x.Result?.IsNotQualified ?? false);
        var visibility = disqualifiedRecord != null
            ? Visibility.Visible
            : Visibility.Collapsed;
        var text = disqualifiedRecord?.Result.ToString();

        return (text, visibility);
    }

    public int Rank { get; }
    public string HorseGenderString { get; }
    public string TotalAverageSpeed { get; }
    public string TotalTime { get; }
    public Visibility DisqualifiedVisibility { get; }
    public string NotQualifiedText { get; }
}
