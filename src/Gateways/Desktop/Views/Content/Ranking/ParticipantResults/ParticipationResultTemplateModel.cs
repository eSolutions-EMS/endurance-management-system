using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.ParticipantResults;

public class ParticipationResultTemplateModel : ParticipantTemplateModelBase
{
    public ParticipationResultTemplateModel(int rank, IEnumerable<Performance> performances) : base(performances)
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
        this.AverageSpeedInKm = this.Performances.Sum(perf => perf.AverageSpeed) / this.Performances.Count;
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
    public TimeSpan TotalLoopSpan { get; }
    public double? AverageSpeedInKm { get; }

    public string HorseGenderString => this.HorseIsStallion ? STALLION : MARE;
    public string TotalLoopSpanString => this.TotalLoopSpan.ToString(@"hh\:mm\:ss"); // TODO: date formats
}
