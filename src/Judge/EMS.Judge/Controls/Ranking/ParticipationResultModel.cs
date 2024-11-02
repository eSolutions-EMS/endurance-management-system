using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.AggregateRoots.Ranking;
using Core.Domain.State.LapRecords;
using Core.Domain.State.Participations;
using EMS.Judge.Common;
using EMS.Judge.Controls.Manager;
using static Core.Localization.Strings;

namespace EMS.Judge.Controls.Ranking;

public class ParticipationResultModel : ParticipationGridModel
{
    public ParticipationResultModel(int rank, Participation participation)
        : base(participation, true)
    {
        this.Rank = rank;
        this.HorseGenderString = participation.Participant.Horse.IsStallion ? STALLION : MARE; // TODO: move in domain

        if (participation.Participant.LapRecords.Any())
        {
            var (ride, rec, total, speed) = RankingRoot.CalculateTotalValues(participation);
            this.TotalAverageSpeed = ValueSerializer.FormatDouble(speed);
            this.TotalTime = ValueSerializer.FormatSpan(total);
            this.RideTime = ValueSerializer.FormatSpan(ride);
            this.RecTime = ValueSerializer.FormatSpan(rec);
        }

        var (notQualified, visibility) = this.HandleNotQualified(
            participation.Participant.LapRecords.ToList()
        );
        this.DisqualifiedVisibility = visibility;
        this.NotQualifiedText = notQualified;
    }

    private (string notQualified, Visibility) HandleNotQualified(List<LapRecord> records)
    {
        var disqualifiedRecord = records.FirstOrDefault(x => x.Result?.IsNotQualified ?? false);
        var visibility = disqualifiedRecord != null ? Visibility.Visible : Visibility.Collapsed;
        var text = disqualifiedRecord?.Result.ToString();

        return (text, visibility);
    }

    public int Rank { get; }
    public string HorseGenderString { get; }
    public string TotalAverageSpeed { get; }
    public string TotalTime { get; }
    public string RideTime { get; }
    public string RecTime { get; }
    public Visibility DisqualifiedVisibility { get; }
    public string NotQualifiedText { get; }
}
