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
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

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

    public string GenerateFeiExport(int competitionId)
    {
        var @event = _stateContext.State.Event;
        if (!_stateContext.State.Event.HasStarted)
        {
            throw Helper.Create<EnduranceEventException>("Event has not started yet");
        }
        var competition = @event.Competitions.FindDomain(competitionId);
        var ctEnduranceCompetitions = CreateCompetitions(competition);
        var horseSport = CreateHorseSport(@event, ctEnduranceCompetitions);

        var xml = BuildXml(horseSport);
        xml = InsertGeneratedDate(xml);

        return xml;
    }

    public static (TimeSpan loop, TimeSpan rec, TimeSpan phase, double? speed) CalculateTotalValues(Participation participation)
    {
        var performances = Performance.GetAll(participation).ToArray();
        var totalLenght = participation.Participant.LapRecords.Sum(x => x.Lap.LengthInKm);

        var loopTIme = performances.Aggregate(TimeSpan.Zero, (result, x) => result + (x.ArrivalTime.Value - x.StartTime));
        var recTime = performances
            .Where(x => !x.Record.Lap.IsFinal && x.RecoverySpan.HasValue)
            .Aggregate(TimeSpan.Zero, (result, x) => result + x.RecoverySpan.Value);
        var phaseTime = loopTIme + recTime;
        var avrageTotalPhaseSpeed = totalLenght / phaseTime.TotalHours;

        return (loopTIme, recTime, phaseTime, avrageTotalPhaseSpeed);
    }

    public IReadOnlyList<CompetitionResultAggregate> Competitions => this._competitions.AsReadOnly();

    private ctEnduranceCompetition CreateCompetitions(Competition competition)
    {
        var ctCompetition = new ctEnduranceCompetition
        {
            FEIID = competition.FeiId,
            ScheduleCompetitionNr = competition.FeiScheduleNumber,
            Rule = competition.Rule,
            Name = competition.Name,
            StartDate = competition.StartTime,
            Team = false,
            ParticipationList = new ctEnduranceParticipations()
        };

        var ctParticipations = CreateParticipations(competition);

        // .. Necessary to order here, because Ranklist implementation is terrible
        ctCompetition.ParticipationList.Participation = ctParticipations.OrderBy(x => x.Position.Rank).ToArray();
        return ctCompetition;
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

            var (loop, rec, phase, speed) = CalculateTotalValues(participation);
            ctParticipation.Total = new ctEnduranceTotal
            {
                AverageSpeed = Round(speed.Value),
                Time = FormatTime(loop),
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
                    PhaseAverageSpeed = Round(performance.AverageSpeedPhase.Value),
                    PhaseTime = FormatTime(performance.Time),
                    RecoveryTime = FormatTime(performance.RecoverySpan),
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

    private HorseSport CreateHorseSport(EnduranceEvent @event, ctEnduranceCompetition ctEnduranceCompetition)
    {
        var ctEnduranceEvent = new ctEnduranceEvent
        {
            FEIID = @event.FeiId,
            Code = @event.FeiCode,
            StartDate = @event.Competitions.OrderBy(x => x.StartTime).First().StartTime,
            EndDate = DateTime.UtcNow,
            NF = "BUL",
            Competitions = [ ctEnduranceCompetition ],
        };
        var horseSport = new HorseSport()
        {
            Generated = new ctGenerated
            {
                Software = "EMS",
                SoftwareVersion = "4.1.3",
                Organization = "NotACompany",
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

    private string BuildXml(HorseSport horseSport)
    {
        var serializer = new XmlSerializer(typeof(HorseSport));
        using var stream = new StringWriter();
        serializer.Serialize(stream, horseSport);
        return stream.ToString();
    }

    private string InsertGeneratedDate(string xml)
    {
        return xml.Replace("<Generated Software", $"<Generated Date=\"{DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture)}+00:00\" Software");
    }

    private decimal Round(double value) => (decimal)Math.Round(value, 2);
    private string FormatTime(TimeSpan? value) => value?.ToString(@"hh\:mm\:ss");
}
