using EnduranceJudge.Domain.AggregateRoots.Rankings;
using EnduranceJudge.Domain.AggregateRoots.Rankings.Aggregates;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.ParticipantResults;
using System;
using System.Collections.ObjectModel;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.RankLists;

public class RankListTemplateModel : ViewModelBase, ICompetitionData
{
    public const string DATE_TIME_FORMAT = "dd.mm.yyyy / HH:mm:ss";
    public const string DATE_ONLY_FORMAT = "dd.mm.yyyy";

    public RankListTemplateModel(RankList rankList, ICompetitionData competitionData)
    {
        this.EventName = competitionData.EventName;
        this.PopulatedPlace = competitionData.PopulatedPlace;
        this.CountryName = competitionData.CountryName;
        this.CompetitionDate = competitionData.CompetitionDate;
        this.Organizer = competitionData.Organizer;
        this.CompetitionName = competitionData.CompetitionName;
        this.CompetitionLengthInKm = competitionData.CompetitionLengthInKm;
        this.PresidentGroundJuryName = competitionData.PresidentGroundJuryName;
        this.ChiefStewardName = competitionData.ChiefStewardName;
        this.DateNow = competitionData.DateNow;

        var rank = 1;
        foreach (var participant in rankList)
        {
            var entry = new ParticipantResultTemplateModel(rank, participant);
            this.RankList.Add(entry);
            rank++;
        }
    }

    public ObservableCollection<ParticipantResultTemplateModel> RankList { get; } = new();

    public string Title => $"{this.CompetitionName} * {this.CompetitionLengthInKm} {KM.ToUpper()}";
    public string EventName { get; }
    public string PopulatedPlace { get; }
    public string CountryName { get; }
    public DateTime CompetitionDate { get; }
    public string Organizer { get; }
    public string CompetitionName { get; }
    public double CompetitionLengthInKm { get; }
    public string PresidentGroundJuryName { get; }
    public string ChiefStewardName { get; }
    public DateTime DateNow { get; }

    public string CompetitionDateString => this.CompetitionDate.ToString(DATE_ONLY_FORMAT);
    public string DateNowString => this.DateNow.ToString(DATE_TIME_FORMAT);
}
