using EMS.Judge.Controls.Manager;
using EMS.Judge.Common;
using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.State.LapRecords;
using Core.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static Core.Localization.Strings;

namespace EMS.Judge.Controls.Ranking;

public class ParticipationResultModel : ParticipationGridModel
{
    public ParticipationResultModel(int rank, Participation participation)
        : base(participation, true)
    {
        this.Distance = null;
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
        var totalLenght = participation.Participant.LapRecords.Sum(x => x.Lap.LengthInKm);
        var totalTime = performances
            .Where(x => x.Time.HasValue)
            .Aggregate(TimeSpan.Zero, (ag, perf) => ag + perf.Time!.Value);
        var averageSpeed = totalLenght / totalTime.TotalHours;
        return (averageSpeed, totalTime);
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
