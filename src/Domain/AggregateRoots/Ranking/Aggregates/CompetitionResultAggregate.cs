using EnduranceJudge.Domain.Core;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;

public class CompetitionResultAggregate : IAggregate, ICompetitionData
{
    internal CompetitionResultAggregate(
        EnduranceEvent enduranceEvent,
        Competition competition,
        IList<Participation> participations)
    {
        this.Id = DomainIdProvider.Generate();
        this.CompetitionLengthInKm = competition.Laps.Aggregate(0d, (total, x) => total + x.LengthInKm);
        this.CompetitionName = competition.Name;
        this.EventName = enduranceEvent.Name;
        this.PopulatedPlace = enduranceEvent.PopulatedPlace;
        this.CountryName = enduranceEvent.Country.Name;
        this.PresidentGroundJuryName = enduranceEvent.PresidentGroundJury?.Name;
        this.ChiefStewardName = enduranceEvent.Stewards.FirstOrDefault()?.Name;
        this.CompetitionName = competition.Name;
        this.CompetitionDate = competition.StartTime;
        this.DateNow = DateTime.Now;
        this.Organizer = "BFKS";

        var kidsRankList = new RankList(Category.Kids, participations);
        var adultsRankList = new RankList(Category.Adults, participations);
        if (kidsRankList.Any())
        {
            this.KidsRankList = kidsRankList;
        }
        if (adultsRankList.Any())
        {
            this.AdultsRankList = adultsRankList;
        }
    }

    public int Id { get; }
    public string EventName { get; }
    public string PopulatedPlace { get; }
    public string Organizer { get; }
    public string CountryName { get; }
    public string PresidentGroundJuryName { get; }
    public string ChiefStewardName { get; }
    public DateTime DateNow { get; }
    public string CompetitionName { get; }
    public DateTime CompetitionDate { get; }
    public double CompetitionLengthInKm { get; }
    public RankList KidsRankList { get; }
    public RankList AdultsRankList { get; }
}
