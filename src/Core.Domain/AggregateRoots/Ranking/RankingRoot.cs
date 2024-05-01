using Core.Domain.AggregateRoots.Ranking.Aggregates;
using Core.Domain.Common.Models;
using Core.Domain.State;
using Core.Domain.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.AggregateRoots.Ranking.Models;
using System;
using Core.Domain.Common.Exceptions;
using Core.Domain.State.EnduranceEvents;
using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.State.Participations;
using Core.Domain.State.Competitions;

namespace Core.Domain.AggregateRoots.Ranking;

public class RankingRoot : IAggregateRoot
{
    private readonly List<CompetitionResultAggregate> _competitions = new();
    private readonly IStateContext _stateContext;

    public RankingRoot(IStateContext stateContext)
    {
        var state = stateContext.State;
        if (state.Event == default)
        {
            return;
        }
        var competitionsIds = state.Participations
            .SelectMany(x => x.CompetitionsIds)
            .Distinct()
            .ToList();
        foreach (var id in competitionsIds)
        {
            var competition = state.Event.Competitions.FindDomain(id);
            var participations = state.Participations
                .Where(x => x.CompetitionsIds.Contains(competition.Id))
                .ToList();
            var listing = new CompetitionResultAggregate(state.Event, competition, participations);
            this._competitions.Add(listing);
        }
        _stateContext = stateContext;
    }

    public CompetitionResultAggregate GetCompetition(int competitionId)
    {
        var aggregate = this._competitions.First(x => x.Id == competitionId);
        return aggregate;
    }

    public HorseSport GenerateFeiExport()
    {
        var @event = _stateContext.State.Event;
        if (!_stateContext.State.Event.HasStarted)
        {
            throw Helper.Create<EnduranceEventException>("Event has not started yet");
        }
        var ctEnduranceCompetitions = CreateCompetitions(@event);
        var horseSport = CreateHorseSport(@event, ctEnduranceCompetitions);
        return horseSport;
    }

    public static (TimeSpan ride, TimeSpan rec, TimeSpan total, double? speed) CalculateTotalValues(Participation participation)
    {
        var performances = Performance.GetAll(participation).ToArray();
        var totalLenght = participation.Participant.LapRecords.Sum(x => x.Lap.LengthInKm);

        var rideTime = performances.Aggregate(TimeSpan.Zero, (result, x) => result + (x.ArrivalTime.Value - x.StartTime));
        var recTime = performances
            .Where(x => !x.Record.Lap.IsFinal)
            .Aggregate(TimeSpan.Zero, (result, x) => result + x.RecoverySpan.Value);
        var totalPhaseTime = rideTime + recTime;
        var avrageTotalPhaseSpeed = totalLenght / totalPhaseTime.TotalHours;

        return (rideTime, recTime, totalPhaseTime, avrageTotalPhaseSpeed);
    }

    public IReadOnlyList<CompetitionResultAggregate> Competitions => this._competitions.AsReadOnly();

    private IEnumerable<ctEnduranceCompetition> CreateCompetitions(EnduranceEvent @event)
    {
        foreach (var comp in @event.Competitions)
        {
            var competition = new ctEnduranceCompetition
            {
                FEIID = comp.FeiId,
                ScheduleCompetitionNr = comp.FeiScheduleNumber,
                Rule = comp.Rule,
                Name = comp.Name,
                StartDate = comp.StartTime,
                Team = false,
                ParticipationList = new ctEnduranceParticipations()
            };

            var ctParticipations = CreateParticipations(comp);

            // .. Necessary to order here, because Ranklist implementation is terrible
            competition.ParticipationList.Participation = ctParticipations.OrderBy(x => x.Position.Rank).ToArray();
            yield return competition;
        }
    }

    private IEnumerable<ctEnduranceIndivResult> CreateParticipations(Competition competition)
    {
        var competitionResult = _competitions.First(x => x.Id == competition.Id);
        foreach (var participation in _stateContext.State.Participations.Where(x => x.CompetitionsIds.Contains(competition.Id)))
        {
            var ranklist = competitionResult.Rank(participation.Participant.Athlete.Category);
            var result = ranklist.FirstOrDefault(x => x.Participant.Number == participation.Participant.Number);
            var athlete = participation.Participant.Athlete;
            var horse = participation.Participant.Horse;

            var ctParticipation = new ctEnduranceIndivResult
            {
                Athlete = new ctEnduranceAthlete
                {
                    FEIID = int.Parse(athlete.FeiId),
                    AthleteNumber = int.Parse(participation.Participant.Number),
                    FirstName = athlete.FirstName,
                    FamilyName = athlete.LastName,
                    CompetingFor = athlete.Country.IsoCode,
                },
                Horse = new ctHorse
                {
                    FEIID = horse.FeiId,
                    Name = horse.Name
                },
                Complement = new ctEnduranceComplement
                {
                    BestCondition = false,
                },
                Position = new ctPositionIndiv
                {
                    Status = result.Participant.LapRecords.Last().Result.TypeCode,
                    Rank = ranklist.IndexOf(result) + 1,
                },
            };

            var ctDays = CreateDaysAndPhases(participation, competition);
            ctParticipation.Phases = ctDays.ToArray();

            var (ride, rec, total, speed) = CalculateTotalValues(participation);
            ctParticipation.Total = new ctEnduranceTotal
            {
                AverageSpeed = Math.Truncate((decimal)speed * 100) / 100,
                Time = total.ToString(@"hh\:mm\:ss"),
            };

            yield return ctParticipation;
        }
    }

    private IEnumerable<ctEnduranceDayResult> CreateDaysAndPhases(Participation participation, Competition competition)
    {
        var days = new List<ctEnduranceDayResult>();
        var day = new ctEnduranceDayResult() { Number = 1 };
        var lastDate = default(DateTime);
        foreach (var record in participation.Participant.LapRecords)
        {
            var performance = new Performance(record, competition.Type, 0);
            var phase = new ctEndurancePhaseResult
            {
                Number = participation.Participant.LapRecords.IndexOf(record) + 1,
                Result = new ctEndurancePhaseResultScore
                {
                    PhaseAverageSpeed = Math.Truncate((decimal)performance.AverageSpeedPhase * 100) / 100,
                    PhaseTime = performance.Time?.ToString(@"hh\:mm\:ss"),
                    RecoveryTime = performance.RecoverySpan?.ToString(@"hh\:mm\:ss"),
                }
            };
            if (lastDate == default || lastDate == record.StartTime.Date)
            {
                var list = new List<ctEndurancePhaseResult>(day.Phase ?? Enumerable.Empty<ctEndurancePhaseResult>())
                {
                    phase
                };
                day.Phase = list.ToArray();
            }
            else
            {
                days.Add(day);
                day = new ctEnduranceDayResult()
                {
                    Phase = new List<ctEndurancePhaseResult> { phase }.ToArray(),
                    Number = day.Number + 1
                };
            }
            lastDate = record.StartTime.Date;
        }
        days.Add(day);
        return days;
    }

    private HorseSport CreateHorseSport(EnduranceEvent @event, IEnumerable<ctEnduranceCompetition> ctEnduranceCompetitions)
    {
        var ctEnduranceEvent = new ctEnduranceEvent
        {
            FEIID = @event.FeiId,
            Code = @event.FeiCode,
            StartDate = @event.Competitions.OrderBy(x => x.StartTime).First().StartTime,
            EndDate = DateTime.UtcNow,
            NF = @event.Country.IsoCode,
            Competitions = ctEnduranceCompetitions.ToArray(),
        };
        var horseSport = new HorseSport()
        {
            Generated = new ctGenerated
            {
                Date = DateTime.UtcNow,
            },
            EventResult = new ctShowResultType
            {
                Show = new ctShowResult
                {
                    Venue = new ctVenue
                    {
                        Name = @event.Name,
                        Country = @event.Country.IsoCode,
                    },
                    EnduranceEvent = new List<ctEnduranceEvent> { ctEnduranceEvent }.ToArray(),
                    StartDate = @event.Competitions.OrderBy(x => x.StartTime).First().StartTime.Date,
                    EndDate = DateTime.UtcNow.Date,
                    FEIID = @event.ShowFeiId,
                }
            }
        };
        return horseSport;
    }
}
