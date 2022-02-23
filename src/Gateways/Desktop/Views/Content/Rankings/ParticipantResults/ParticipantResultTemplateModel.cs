using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common;
using System;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.ParticipantResults;

public class ParticipantResultTemplateModel : ParticipantTemplateModelBase
{
    public ParticipantResultTemplateModel(int rank, Participant participant) : base(participant)
    {
        this.Rank = rank;
        this.ParticipantNumber = participant.Number;
        this.AthleteName = participant.Athlete.Name;
        this.AthleteFeiId = participant.Athlete.FeiId;
        this.AthleteCountryName = participant.Athlete.Country.Name;
        this.HorseName = participant.Horse.Name;
        this.HorseFeiId = participant.Horse.FeiId;
        this.HorseIsStallion = participant.Horse.IsStallion;
        this.HorseBreed = participant.Horse.Breed;
        this.TrainerFeiId = participant.Horse.TrainerFeiId;
        this.TrainerName = participant.Horse.TrainerName;
        this.AverageSpeedInKm = Math.Round(participant.Participation.AverageSpeedForLoopInKm, 3);
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
    public double AverageSpeedInKm { get; }

    public string HorseGenderString => this.HorseIsStallion ? STALLION : MARE;
    public string TotalLoopSpanString => this.TotalLoopSpan.ToString(@"hh\:mm\:ss");
}
