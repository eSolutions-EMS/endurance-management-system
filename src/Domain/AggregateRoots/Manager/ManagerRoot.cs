using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.AggregateRoots.Manager;

public class ManagerRoot : IAggregateRoot
{
    private readonly IState state;

    public ManagerRoot()
    {
        this.state = StaticProvider.GetService<IState>();
        // TODO: think this is no longer necessary.
        // Check is necessary due to Prism's initialization logic which uses reflection
        // to generate instances of views as part of the startup process.
        // These views are not used in the actual views during the application use cycle
        if (this.state?.Event == null)
        {
            return;
        }
    }

    public bool HasStarted()
        => this.state.Participations.Any(x => x.Participant.TimeRecords.Any());

    public void Start()
    {
        this.ValidateConfiguration();
        var participations = this.state
            .Participations
            .Select(x => new ParticipationsAggregate(x))
            .ToList();
        foreach (var participation in participations)
        {
            participation.Start();
        }
        this.state.Event.HasStarted = true;
    }

    public void UpdateRecord(int number, DateTime time)
    {
        var participation = this.GetParticipation(number);
        participation.UpdateRecord(time);
    }

    public void CompletePerformance(int number, string code)
    {
        var participation = this.GetParticipation(number);
        var record = participation.GetCurrent();
        record.Complete(code);
    }

    public void ReInspection(int number, bool isRequired)
    {
        var participation = this.GetParticipation(number);
        var record = participation.GetCurrent();
        if (record == null)
        {
            throw Helper.Create<ParticipantException>(PARTICIPANT_HAS_NO_ACTIVE_RECORD_MESSAGE, number);
        }
        record!.ReInspection(isRequired);
    }

    public void RequireInspection(int number, bool isRequired)
    {
        var participant = this.GetParticipation(number);
        var performance = participant.GetCurrent();
        if (performance == null)
        {
            throw Helper.Create<ParticipationException>(PARTICIPANT_HAS_NO_ACTIVE_RECORD_MESSAGE, number);
        }
        performance!.RequireInspection(isRequired);
    }

    public Performance EditRecord(ILapRecordState state)
    {
        var record = this.state
            .Participations
            .Select(x => x.Participant)
            .SelectMany(part => part.TimeRecords)
            .FirstOrDefault(perf => perf.Equals(state));
        var manager = new LapRecordsAggregate(record);
        manager.Edit(state);
        var performance = this.GetPerformance(state.Id);
        return performance;
    }

    public IEnumerable<StartModel> GetStartList(bool includePast)
    {
        // TODO: check
        var participations = this.state.Participations;
        var startList = new Startlist(participations, includePast);
        return startList;
    }

    private ParticipationsAggregate GetParticipation(int number)
    {
        var participation = this.state
            .Participations
            .FirstOrDefault(x => x.Participant.Number == number);
        if (participation == null)
        {
            throw Helper.Create<ParticipantException>(PARTICIPANT_NUMBER_NOT_FOUND_MESSAGE, number);
        }
        var aggregate = new ParticipationsAggregate(participation);
        return aggregate;
    }

    private Performance GetPerformance(int id)
    {
        foreach (var participant in this.state.Participants)
        {
            foreach (var timeRecord in participant.TimeRecords)
            {
                var participation = this.state.Participations.FirstOrDefault(x => x.Participant.Id == participant.Id);
                if (participation == null)
                {
                    throw Helper.Create<ParticipationException>(NOT_FOUND_BY_ID_MESSAGE);
                }
                var competition = participation.CompetitionConstraint;
                var index = participant.TimeRecords.ToList().IndexOf(timeRecord);
                var performance = new Performance(participant, competition.Laps, index);
                return performance;
            }
        }
        throw Helper.Create<LapRecordException>(NOT_FOUND_BY_ID_MESSAGE, id);
    }

    private void ValidateConfiguration()
    {
        foreach (var competition in this.state.Event.Competitions)
        {
            if (competition.Laps.All(x => !x.IsFinal))
            {
                throw Helper.Create<CompetitionException>(
                    INVALID_COMPETITION_NO_FINAL_PHASE_MESSAGE,
                    competition.Name);
            }
        }
        foreach (var participation in this.state.Participations)
        {
            if (!participation.CompetitionsIds.Any())
            {
                throw Helper.Create<ParticipantException>(
                    INVALID_PARTICIPANT_NO_PARTICIPATIONS_MESSAGE,
                    participation.Participant.Number);
            }
            if (participation.Participant.Athlete.Country == null)
            {
                throw Helper.Create<ParticipantException>(
                    INVALID_PARTICIPANT_NO_COUNTRY_MESSAGE,
                    participation.Participant.Number);
            }
        }
    }
}
