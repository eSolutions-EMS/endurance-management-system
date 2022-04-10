﻿using EnduranceJudge.Domain.AggregateRoots.Ranking;
using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.ParticipantResults;
using System;
using System.Collections.ObjectModel;
using static EnduranceJudge.Localization.Strings;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;
namespace EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.RankLists;

public class RankListTemplateModel : ViewModelBase, ICompetitionData
{
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
        this.FeiTechDelegateName = competitionData.FeiTechDelegateName;
        this.PresidentVetCommitteeName = competitionData.PresidentVetCommitteeName;
        this.FeiVetDelegateName = competitionData.FeiVetDelegateName;
        this.DateNow = competitionData.DateNow;

        var rank = 1;
        foreach (var participation in rankList)
        {
            var entry = new ParticipationResultTemplateModel(rank, participation);
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
    public string FeiTechDelegateName { get; }
    public DateTime DateNow { get; }
    public string PresidentVetCommitteeName { get; }
    public string FeiVetDelegateName { get; }
    public string CompetitionDateString => this.CompetitionDate.ToString(DATE_ONLY_FORMAT);
    public string DateNowString => this.DateNow.ToString(DATE_ONLY_FORMAT);
}
