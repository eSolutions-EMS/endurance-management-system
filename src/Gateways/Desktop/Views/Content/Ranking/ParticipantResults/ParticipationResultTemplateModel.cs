using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.ParticipantResults;

public class ParticipationResultTemplateModel : ParticipantTemplateModelBase
{
    public ParticipationResultTemplateModel(int rank, List<Performance> performances) : base(performances)
    {
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
        var totalAverageSpeed = performances.Sum(perf => perf.AverageSpeed) / this.Performances.Count;
        var totalTime = performances
            .Where(x => x.Time.HasValue)
            .Aggregate(TimeSpan.Zero, (ag, perf) => ag + perf.Time!.Value);
        this.TotalAverageSpeedString = ValueSerializer.FormatDouble(totalAverageSpeed);
        this.TotalTime = ValueSerializer.FormatSpan(totalTime);
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
}
