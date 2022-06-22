using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.AggregateRoots.Manager.WitnessEvents;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.AggregateRoots.Manager;

public class ManagerRoot : IAggregateRoot
{
    private readonly IState state;

    public ManagerRoot(IStateContext context)
    {
        this.state = context.State;
        Witness.Events += this.Handle;
    }

    private void Handle(object sender, WitnessEvent witnessEvent)
    {
        if (witnessEvent.Type == WitnessEventType.Finish)
        {
            this.HandleWitnessFinish(witnessEvent.TagId, witnessEvent.Time);
        }
        if (witnessEvent.Type == WitnessEventType.EnterVet)
        {
            this.HandleWitnessVet(witnessEvent.TagId, witnessEvent.Time);
        }
        // TODO: Make sure save is persisted. Maybe transition to event-driven persistance?
    }
    
    public bool HasStarted()
        => this.state.Participations.Any(x => x.Participant.LapRecords.Any());

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
        var participation = this
            .GetParticipation(number)
            .Aggregate();
        participation.Update(time);
    }
    public void HandleWitnessFinish(string rfid, DateTime time)
    {
        var participation = this
            .GetParticipation(rfid)
            .Aggregate();
        if (participation.CurrentLap.ArrivalTime != null && participation.CurrentLap.Result == null)
        {
            // TODO: fix/remove
            Helper.Create<ParticipantException>("cannot finish. 'ArriveTime' is not null and Lap is not completed");
        }
        participation.Update(time);
    } 
    public void HandleWitnessVet(string rfid, DateTime time)
    {
        var participation = this
            .GetParticipation(rfid)
            .Aggregate();

        if (participation.CurrentLap.Result != null)
        {
            // TODO fix/remove
            Helper.Create<ParticipantException>("cannot record VET. 'CurrentLap' is completed");
        }
        if (participation.CurrentLap.InspectionTime != null && participation.CurrentLap.ReInspectionTime != null)
        {
            // TODO fix/remove
            var message = "cannot record VET. 'InspectionTime' amd 'ReInspectionTime' are not null"; 
            Helper.Create<ParticipantException>(message);
        }
        participation.Update(time);
    }
    public void Disqualify(int number, string reason)
    {
        reason ??= nameof(_DQ);
        var lap = this.GetLastLap(number);
        lap.Disqualify(reason);
    }
    public void FailToQualify(int number, string reason)
    {
        if (string.IsNullOrEmpty(reason))
        {
            throw Helper.Create<ParticipantException>(PARTICIPANT_CANNOT_FTQ_WITHOUT_REASON_MESSAGE, _FTQ);
        }
        var lap = this.GetLastLap(number);
        lap.FailToQualify(reason);
    }
    public void Resign(int number, string reason)
    {
        reason ??= nameof(_RET);
        var lap = this.GetLastLap(number);
        lap.Resign(reason);
    }

    private LapRecordsAggregate GetLastLap(int participantNumber)
    {
        var participation = this.GetParticipation(participantNumber);
        var participationsAggregate = participation.Aggregate();
        var lap = participationsAggregate.CurrentLap;
        return lap.Aggregate();
    }

    public void ReInspection(int number, bool isRequired)
    {
        var participation = this.GetParticipation(number);
        var lastRecord = participation
            .Aggregate()
            .CurrentLap
            .Aggregate();
        lastRecord!.ReInspection(isRequired);
    }

    public void RequireInspection(int number, bool isRequired)
    {
        var participation = this.GetParticipation(number);
        var last = participation
            .Aggregate()
            .CurrentLap
            .Aggregate();
        last.RequireInspection(isRequired);
    }

    public Performance EditRecord(ILapRecordState state)
    {
        var participation = this.state
            .Participations
            .First(x => x.Participant.LapRecords.Contains(state));
        var records = participation.Participant.LapRecords.ToList();
        var record = records.First(rec => rec.Equals(state));
        var aggregate = new LapRecordsAggregate(record);
        aggregate.Edit(state);
        var performance = new Performance(participation, records.IndexOf(record));
        return performance;
    }

    public IEnumerable<StartModel> GetStartList(bool includePast)
    {
        var participations = this.state.Participations;
        var startList = new Startlist(participations, includePast);
        return startList;
    }

    private Participation GetParticipation(int number)
    {
        var participation = this.state
            .Participations
            .FirstOrDefault(x => x.Participant.Number == number);
        if (participation == null)
        {
            throw Helper.Create<ParticipantException>(NOT_FOUND_MESSAGE, NUMBER, number);
        }
        return participation;
    }

    private Participation GetParticipation(string rfid)
    {
        var participation = this.state
            .Participations
            .FirstOrDefault(x => x.Participant.RfId == rfid);
        if (participation == null)
        {
            throw Helper.Create<ParticipantException>(NOT_FOUND_MESSAGE, RFID_NUMBER, rfid);
        }
        return participation;
    }

    private void ValidateConfiguration()
    {
        var competitionWithoutLaps = this.state.Event.Competitions.FirstOrDefault(comp => comp.Laps.Count == 0);
        if (competitionWithoutLaps != null)
        {
            throw Helper.Create<CompetitionException>(
                COMPETITION_CANNOT_START_WITHOUT_PHASES_MESSAGES,
                competitionWithoutLaps.Name);
        }
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
