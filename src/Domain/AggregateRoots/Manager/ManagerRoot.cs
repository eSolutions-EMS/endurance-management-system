using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.Events;
using EnduranceJudge.Core.Services;
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
using EnduranceJudge.Domain.AggregateRoots.Manager.WitnessEvents;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.AggregateRoots.Manager;

public class ManagerRoot : IAggregateRoot
{
    public static string dataDirectoryPath;
    private readonly IState state;
    private readonly IFileService file;
    private readonly IJsonSerializationService serialization;
    
    public ManagerRoot(IStateContext context)
    {
        // TODO: fix log
        this.file = StaticProvider.GetService<IFileService>();
        this.serialization = StaticProvider.GetService<IJsonSerializationService>();
        this.state = context.State;
        Witness.Events += this.Handle;
    }

    private void Handle(object sender, WitnessEvent witnessEvent)
    {
        if (witnessEvent.TagId == "test")
        {
            return;
        }
        this.Log(witnessEvent);
        try
        {
            if (witnessEvent.Type == WitnessEventType.Arrival)
            {
                this.HandleArrive(witnessEvent.TagId, witnessEvent.Time);
            }
            if (witnessEvent.Type == WitnessEventType.VetIn)
            {
                this.HandleVet(witnessEvent.TagId, witnessEvent.Time);
            }
            Witness.RaiseStateChanged();
        }
        catch (Exception exception)
        {
            CoreEvents.RaiseError(exception);
        }
    }

    private void Log(WitnessEvent witnessEvent)
    {
        var timestamp = DateTime.Now.ToString("HH-mm-ss");
        var log = new Dictionary<string, object>
        {
            { "tag-id", witnessEvent.TagId },
            { "type", witnessEvent.Type.ToString() },
            { "time", witnessEvent.Time },
        };
        var serialized = this.serialization.Serialize(log);
        var filename = $"witness_{timestamp}-{witnessEvent.Type}-{witnessEvent.TagId}.json";
        var path = $"{dataDirectoryPath}/{filename}";
        this.file.Create(path, serialized);
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

    public void UpdateRecord(string number, DateTime time)
    {
        var participation = this
            .GetParticipation(number)
            .Aggregate();
        var currentLap = participation.CurrentLap.Aggregate();
        if (currentLap.IsComplete || participation.CurrentLap.ArrivalTime == null)
        {
            participation.Arrive(time);
        }
        else
        {
            participation.Vet(time);
        }
    }

    private readonly Dictionary<string, DateTime> arrivalCache = new();
    private readonly Dictionary<string, DateTime> vetCache = new();

    public void HandleArrive(string rfid, DateTime time)
    {
        var participation = this
            .GetParticipation(rfid)
            .Aggregate();
        if (participation.CurrentLap.ArrivalTime != null && participation.CurrentLap.Result == null)
        {
            return;
        }
        // TODO: extract deduplication logic in common utility
        // Make sure that we only finish once even if we detect both tags
        var now = DateTime.Now;
        if (this.arrivalCache.ContainsKey(participation.Number) 
            && now - this.arrivalCache[participation.Number] < TimeSpan.FromSeconds(30))
        {
            return;
        }
        this.arrivalCache[participation.Number] = now;
        participation.Arrive(time);
    }
    public void HandleVet(string rfid, DateTime time)
    {
        var participation = this
            .GetParticipation(rfid)
            .Aggregate();
        if (participation.CurrentLap.Result != null
            || participation.CurrentLap.InspectionTime != null && participation.CurrentLap.ReInspectionTime != null)
        {
            return;
        }
        var now = DateTime.Now;
        if (this.vetCache.ContainsKey(participation.Number) 
            && now - this.vetCache[participation.Number] < TimeSpan.FromSeconds(30))
        {
            return;
        }
        this.vetCache[participation.Number] = now;
        participation.Vet(time);
    }
    
    public void Disqualify(string number, string reason)
    {
        reason ??= nameof(_DQ);
        var lap = this.GetLastLap(number);
        lap.Disqualify(reason);
    }
    public void FailToQualify(string number, string reason)
    {
        if (string.IsNullOrEmpty(reason))
        {
            throw Helper.Create<ParticipantException>(PARTICIPANT_CANNOT_FTQ_WITHOUT_REASON_MESSAGE, _FTQ);
        }
        var lap = this.GetLastLap(number);
        lap.FailToQualify(reason);
    }
    public void Resign(string number, string reason)
    {
        reason ??= nameof(_RET);
        var lap = this.GetLastLap(number);
        lap.Resign(reason);
    }

    private LapRecordsAggregate GetLastLap(string participantNumber)
    {
        var participation = this.GetParticipation(participantNumber);
        var participationsAggregate = participation.Aggregate();
        var lap = participationsAggregate.CurrentLap;
        return lap.Aggregate();
    }

    public void RequireReInspection(string number, bool isRequired)
    {
        var participation = this.GetParticipation(number);
        var lastRecord = participation
            .Aggregate()
            .CurrentLap
            .Aggregate();
        lastRecord!.RequireReInspection(isRequired);
    }

    public void RequireCompulsoryInspection(string number, bool isRequired)
    {
        var participation = this.GetParticipation(number);
        var last = participation
            .Aggregate()
            .CurrentLap
            .Aggregate();
        last.RequireCompulsoryInspection(isRequired);
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
        return startList.List;
    }

    private Participation GetParticipation(string numberOrTag)
    {
        var participation = this.state
            .Participations
            .FirstOrDefault(x => x.Participant.Number == numberOrTag
                || x.Participant.RfIdHead == numberOrTag
                || x.Participant.RfIdNeck == numberOrTag);
        if (participation == null)
        {
            throw Helper.Create<ParticipantException>(NOT_FOUND_MESSAGE, NUMBER, numberOrTag);
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
