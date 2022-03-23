using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.AggregateRoots.Ranking;
using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.ParticipantResults;
using System;
using System.Collections.ObjectModel;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.RankLists;

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
        this.Name = competitionData.Name;
        this.CompetitionLengthInKm = competitionData.CompetitionLengthInKm;
        this.PresidentGroundJuryName = competitionData.PresidentGroundJuryName;
        this.ChiefStewardName = competitionData.ChiefStewardName;
        this.DateNow = competitionData.DateNow;

        var rank = 1;
        foreach (var participation in rankList)
        {
            var performances = Performance.GetAll(participation);
            var entry = new ParticipationResultTemplateModel(rank, performances);
            this.RankList.Add(entry);
            rank++;
        }
    }

    public ObservableCollection<ParticipationResultTemplateModel> RankList { get; } = new();

    public string Title => $"{this.Name} * {this.CompetitionLengthInKm} {KM.ToUpper()}";
    public string EventName { get; }
    public string PopulatedPlace { get; }
    public string CountryName { get; }
    public DateTime CompetitionDate { get; }
    public string Organizer { get; }
    public string Name { get; }
    public double CompetitionLengthInKm { get; }
    public string PresidentGroundJuryName { get; }
    public string ChiefStewardName { get; }
    public DateTime DateNow { get; }

    public string CompetitionDateString => this.CompetitionDate.ToString(DATE_ONLY_FORMAT);
    public string DateNowString => this.DateNow.ToString(DATE_TIME_FORMAT);
}
