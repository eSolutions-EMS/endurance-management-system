using Core.Domain.Common.Models;
using Core.Domain.Enums;
using Core.Domain.State.Competitions;
using Core.Domain.State.EnduranceEvents;
using Core.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.AggregateRoots.Ranking.Aggregates;

public class CompetitionResultAggregate : IAggregate, ICompetitionData
{
    private readonly List<Participation> participations;
    internal CompetitionResultAggregate(
        EnduranceEvent enduranceEvent,
        Competition competition,
        List<Participation> participations)
    {
        this.Id = competition.Id;
        this.CompetitionLengthInKm = competition.Laps.Aggregate(0d, (total, x) => total + x.LengthInKm);
        this.Name = competition.Name;
        this.EventName = enduranceEvent.Name;
        this.PopulatedPlace = enduranceEvent.PopulatedPlace;
        this.CountryName = enduranceEvent.Country.Name;
        this.PresidentGroundJuryName = enduranceEvent.PresidentGroundJury?.Name;
        this.FeiTechDelegateName = enduranceEvent.FeiTechDelegate?.Name;
        this.PresidentVetCommitteeName = enduranceEvent.PresidentVetCommittee?.Name;
        this.FeiVetDelegateName = enduranceEvent.FeiVetDelegate?.Name;
        this.Name = competition.Name;
        this.CompetitionDate = competition.StartTime;
        this.DateNow = DateTime.Now;
        this.Organizer = "BFKS";
        this.participations = participations;
    }

    public int Id { get; }
    public string EventName { get; }
    public string PopulatedPlace { get; }
    public string Organizer { get; }
    public string CountryName { get; }
    public string PresidentGroundJuryName { get; }
    public string PresidentVetCommitteeName { get; }
    public string FeiVetDelegateName { get; }
    public string FeiTechDelegateName { get; }
    public DateTime DateNow { get; }
    public string Name { get; }
    public DateTime CompetitionDate { get; }
    public double CompetitionLengthInKm { get; }

    public RanklistAggregate Rank(Category category)
        => new(category, this.participations);
}
