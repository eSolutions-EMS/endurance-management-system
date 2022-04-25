using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common;
using System;
using System.Linq;
using System.Windows;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.ParticipantResults;

public class ParticipationResultTemplateModel : ParticipantTemplateModelBase
{
    public ParticipationResultTemplateModel(int rank, Participation participation) : base(participation)
    {
        var performances = Performance.GetAll(participation).ToList();
        this.Rank = rank;
        this.ParticipantNumber = this.Participant.Number;
        this.AthleteName = this.Participant.Athlete.Name;
        this.AthleteFeiId = this.Participant.Athlete.FeiId;
        this.AthleteCountryName = this.Participant.Athlete.Country.Name;
        this.HorseName = this.Participant.Horse.Name;
        this.HorseFeiId = this.Participant.Horse.FeiId;
        this.HorseIsStallion = this.Participant.Horse.IsStallion;
        this.HorseBreed = this.Participant.Horse.Breed;
        this.TrainerFeiId = this.Participant.Horse.TrainerFeiId;
        this.TrainerName = this.Participant.Horse.TrainerName;
        var totalAverageSpeed = performances.Sum(perf => perf.AverageSpeed) / performances.Count;
        var totalTime = performances
            .Where(x => x.Time.HasValue)
            .Aggregate(TimeSpan.Zero, (ag, perf) => ag + perf.Time!.Value);
        this.TotalAverageSpeedString = ValueSerializer.FormatDouble(totalAverageSpeed);
        this.TotalTime = ValueSerializer.FormatSpan(totalTime);
        var disqualifiedRecord = participation.Participant.LapRecords.FirstOrDefault(x =>
            x.Result?.IsDisqualified ?? false);
        this.DisqualifiedVisibility = disqualifiedRecord != null
            ? Visibility.Visible
            : Visibility.Collapsed;
        this.DisqualifiedReason = disqualifiedRecord?.Result.Code;
    }

    public int Rank { get; }
    public int ParticipantNumber { get; }
    public string AthleteName { get; }
    public string AthleteFeiId { get; }
    public string AthleteCountryName { get; }
    public string HorseName { get; }
    public string HorseFeiId { get; }
    public bool HorseIsStallion { get; }
    public string HorseBreed { get; }
    public string TrainerFeiId { get; }
    public string TrainerName { get; }
    public string TotalAverageSpeedString { get; }

    public string HorseGenderString => this.HorseIsStallion ? STALLION : MARE;
    public string TotalTime { get; }
    public Visibility DisqualifiedVisibility { get; }
    public string DisqualifiedReason { get; }
}
