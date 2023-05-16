using EMS.Core.Domain.AggregateRoots.Common.Performances;
using EMS.Core.Domain.AggregateRoots.Manager.Aggregates;
using EMS.Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Core.Domain.AggregateRoots.Manager.WitnessEvents;
using EMS.Core.Domain.Core.Exceptions;
using EMS.Core.Domain.Core.Models;
using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Competitions;
using EMS.Core.Domain.State.LapRecords;
using EMS.Core.Domain.State.Participants;
using EMS.Core.Domain.State.Participations;
using EMS.Judge.Application.Core.Services;
using EMS.Core.Events;
using EMS.Core.Services;
using EMS.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using static EMS.Core.Localization.Strings;

namespace EMS.Core.Domain.AggregateRoots.Manager;

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
        try
        {
            var number = this.GetParticipation(witnessEvent.TagId)?.Participant.Number;
            if (number == null)
            {
                return;
            }
            this.Log(witnessEvent, number);
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

    private void Log(WitnessEvent witnessEvent, string number)
    {
        var timestamp = DateTime.Now.ToString("HH-mm-ss");
        var log = new Dictionary<string, object>
        {
            { "tag-id", witnessEvent.TagId },
            { "type", witnessEvent.Type.ToString() },
            { "time", witnessEvent.Time },
        };
        var serialized = this.serialization.Serialize(log);
        var filename = $"witness_{timestamp}-{witnessEvent.Type}-{number}-{witnessEvent.TagId}.json";
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
            Witness.RaiseStartlistChanged(this.GetStartList(false));
        }
    }

    private readonly Dictionary<string, DateTime> arrivalCache = new();
    private readonly Dictionary<string, DateTime> vetCache = new();

    public void HandleArrive(string numberOrTag, DateTime time)
    {
        var participation = this.GetParticipation(numberOrTag);
        var aggregate = participation.Aggregate();
        if (aggregate.CurrentLap.ArrivalTime != null && aggregate.CurrentLap.Result == null)
        {
            return;
        }
        // TODO: extract deduplication logic in common utility
        // Make sure that we only finish once even if we detect both tags
        var now = DateTime.Now;
        if (this.arrivalCache.ContainsKey(aggregate.Number)
            && now - this.arrivalCache[aggregate.Number] < TimeSpan.FromSeconds(30))
        {
            return;
        }
        this.arrivalCache[aggregate.Number] = now;
        this.AddStats(participation, numberOrTag, WitnessEventType.Arrival);
        aggregate.Arrive(time);
    }
    public void HandleVet(string numberOrTag, DateTime time)
    {
        var participation = this.GetParticipation(numberOrTag);
        var aggregate = participation.Aggregate();
        if (aggregate.CurrentLap.Result != null
            || aggregate.CurrentLap.InspectionTime != null && aggregate.CurrentLap.ReInspectionTime != null)
        {
            return;
        }
        var now = DateTime.Now;
        if (this.vetCache.ContainsKey(aggregate.Number)
            && now - this.vetCache[aggregate.Number] < TimeSpan.FromSeconds(30))
        {
            return;
        }
        this.vetCache[aggregate.Number] = now;
        this.AddStats(participation, numberOrTag, WitnessEventType.VetIn);
        aggregate.Vet(time);
        Witness.RaiseStartlistChanged(this.GetStartList(false));
    }

    private void AddStats(Participation participation, string numberOrTag, WitnessEventType type)
    {
        var lastLap = participation.Participant.LapRecords.Last();
        var index = participation.Participant.LapRecords.IndexOf(lastLap);
        if (participation.Participant.RfIdNeck == numberOrTag)
        {
            participation.Participant.DetectedNeck[type].Add(index);
        }
        if (participation.Participant.RfIdHead == numberOrTag)
        {
            participation.Participant.DetectedHead[type].Add(index);
        }
    }

    public void Disqualify(string number, string reason)
    {
        reason ??= nameof(DQ);
        var lap = this.GetLastLap(number);
        lap.Disqualify(reason);
    }
    public void FailToQualify(string number, string reason)
    {
        if (string.IsNullOrEmpty(reason))
        {
            throw Helper.Create<ParticipantException>(PARTICIPANT_CANNOT_FTQ_WITHOUT_REASON_MESSAGE, FTQ);
        }
        var lap = this.GetLastLap(number);
        lap.FailToQualify(reason);
    }
    public void Resign(string number, string reason)
    {
        reason ??= nameof(RET);
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
        aggregate.CheckForResult(
            participation.Participant.MaxAverageSpeedInKmPh,
            participation.CompetitionConstraint.Type);
        participation.RaiseUpdate();
        var previousLength = records
            .Take(records.IndexOf(record))
            .Sum(x => x.Lap.LengthInKm);
        var performance = new Performance(record, participation.CompetitionConstraint.Type, previousLength);
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
        // if (participation == null)
        // {
        //     throw Helper.Create<ParticipantException>(NOT_FOUND_MESSAGE, NUMBER, numberOrTag);
        // }
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
