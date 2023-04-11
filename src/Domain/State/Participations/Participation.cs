using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.State.Participations;

public class Participation : DomainBase<ParticipationException>
{
    internal Participation() {}
    internal Participation(Participant participant, Competition competition) : base(GENERATE_ID)
    {
        this.Participant = participant;
        this.CompetitionConstraint = competition;
        this.competitionsIds.Add(competition.Id);
    }

    public static EventHandler<Participation> UpdateEvent;
    internal void RaiseUpdate()
    {
        UpdateEvent?.Invoke(null, this);
    }

    private List<int> competitionsIds = new();
    public Participant Participant { get; private set; }
    public Competition CompetitionConstraint { get; internal set; }
    public WitnessEventType UpdateType { get; internal set; }

    public double? Distance

        => this.CompetitionConstraint
            ?.Laps
            .Select(x => x.LengthInKm)
            .Sum();

    internal void Add(Competition competition)
    {
        if (this.CompetitionConstraint == null)
        {
            this.CompetitionConstraint = competition;
        }
        this.competitionsIds.Add(competition.Id);
    }
    internal void Remove(int competitionId)
    {
        this.competitionsIds.Remove(competitionId);
        if (!this.CompetitionsIds.Any())
        {
            this.CompetitionConstraint = null;
        }
    }

    public IReadOnlyList<int> CompetitionsIds
    {
        get => this.competitionsIds.AsReadOnly();
        private set => this.competitionsIds = value.ToList();
    }

    public void __REMOVE_PERFORMANCES__()
    {
        this.Participant.__REMOVE_RECORDS__();
    }
}
