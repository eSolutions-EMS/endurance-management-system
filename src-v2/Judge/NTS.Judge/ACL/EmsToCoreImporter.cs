using NTS.Compatibility.EMS;
using NTS.Domain.Core.Entities;
using NTS.Domain.Objects;
using Not.Serialization;
using NTS.Compatibility.EMS.Entities.EnduranceEvents;
using static NTS.Domain.Enums.OfficialRole;
using NTS.Compatibility.EMS.Entities.Competitions;
using NTS.Domain.Enums;
using NTS.Judge.ACL.Factories;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Compatibility.EMS.Enums;
using Not.Application.Ports.CRUD;
using Not.Injection;
using NTS.Domain.Core.Objects;
using Not.DateAndTime;

namespace NTS.Judge.ACL;

public class EmsToCoreImporter : IEmsToCoreImporter
{
    private readonly IRepository<Event> _events;
    private readonly IRepository<Official> _officials;
    private readonly IRepository<Participation> _participations;
    private readonly IRepository<Ranking> _classfications;

    public EmsToCoreImporter(
        IRepository<Event> events,
        IRepository<Official> officials,
        IRepository<Participation> participations,
        IRepository<Ranking> classfications)
    {
        _events = events;
        _officials = officials;
        _participations = participations;
        _classfications = classfications;
    }

    public async Task Import(string emsJson, bool adjustTime = true)
    {
        var existingEvent = await _events.Read(default);
        if (existingEvent != null)
        {
            throw new Exception($"Cannot import data as Event already exists: '{existingEvent}'");
        }

        var emsState = emsJson.FromJson<EmsState>();

        var @event = CreateEvent(emsState.Event, adjustTime);
        //TODOL interesting why some imports fail without ToList? 2024-vakarel (finished) for example
        var officials = CreateOfficials(emsState.Event).ToList();
        var participations = CreateParticipations(emsState, adjustTime).ToList();
        var classifications = CreateClassifications(emsState, adjustTime).ToList();

        await _events.Create(@event);
        foreach (var official in officials)
        {
            await _officials.Create(official);
        }
        foreach (var participation in participations)
        {
            await _participations.Create(participation);
        }
        foreach (var classification in classifications)
        {
            await _classfications.Create(classification);
        }
    }

    private Event CreateEvent(EmsEnduranceEvent emsEvent, bool adjustTime)
    {
        var country = new Country(emsEvent.Country.IsoCode, emsEvent.Country.Name);
        var startTime = emsEvent.Competitions.OrderBy(x => x.StartTime).First().StartTime;
        if (adjustTime)
        {
            startTime = DateTimeHelper.DateTimeNow.AddHours(-1);
        }
        return new Event(
            country,
            emsEvent.PopulatedPlace,
            "populated place",
            new DateTimeOffset(startTime),
            new DateTimeOffset(startTime + TimeSpan.FromHours(12)),
            null, null, null);
    }

    private IEnumerable<Official> CreateOfficials(EmsEnduranceEvent emsEvent)
    {
        var result = new List<Official>();
        if (emsEvent.PresidentGroundJury != null)
        {
            result.Add(new (new Person(emsEvent.PresidentGroundJury.Name), PresidentGroundJury));
        }
        if (emsEvent.PresidentVetCommittee != null)
        {
            result.Add(new(new Person(emsEvent.PresidentVetCommittee.Name), PresidentVet));
        }
        if (emsEvent.FeiTechDelegate != null)
        {
            result.Add(new(new Person(emsEvent.FeiTechDelegate.Name), FeiTechDelegate));
        }
        if (emsEvent.FeiVetDelegate != null)
        {
            result.Add(new(new Person(emsEvent.FeiVetDelegate.Name), FeiVetDelegate));
        }
        if (emsEvent.ForeignJudge != null)
        {
            result.Add(new(new Person(emsEvent.ForeignJudge.Name), ForeignJudge));
        }
        if (emsEvent.ActiveVet != null)
        {
            result.Add(new(new Person(emsEvent.ActiveVet.Name), ActiveVet));
        }
        foreach (var jury in emsEvent.MembersOfJudgeCommittee)
        {
            result.Add(new (new Person(jury.Name), MemberJudge));
        };
        foreach (var vet in emsEvent.MembersOfVetCommittee)
        {
            result.Add(new(new Person(vet.Name), MemberVet));
        }
        return result;
    }

    private IEnumerable<Participation> CreateParticipations(EmsState state, bool adjustTime)
    {
        foreach (var emsParticipation in state.Participations)
        {
            foreach (var competitionId in emsParticipation.CompetitionsIds)
            {
                var competition = state.Event.Competitions.First(x => x.Id == competitionId);
                yield return ParticipationFactory.CreateCore(emsParticipation, competition, adjustTime);
            }
        }
    }

    private IEnumerable<Ranking> CreateClassifications(EmsState state, bool adjustTime)
    {
        var result = new List<Ranking>();
        var entriesforClassification = new Dictionary<EmsCompetition, Dictionary<AthleteCategory, List<RankingEntry>>>();
        foreach (var emsParticipation in state.Participations)
        {
            foreach (var competitionId in emsParticipation.CompetitionsIds)
            {
                var competition = state.Event.Competitions.First(x => x.Id == competitionId);
                var participation = ParticipationFactory.CreateCore(emsParticipation, competition, adjustTime);
                var category = EmsCategoryToAthleteCategory(emsParticipation.Participant.Athlete.Category);
                var entry = new RankingEntry(participation, !emsParticipation.Participant.Unranked);
                if (entriesforClassification.ContainsKey(competition) && entriesforClassification[competition].ContainsKey(category))
                {
                    entriesforClassification[competition][category].Add(entry);
                }
                else if (entriesforClassification.ContainsKey(competition))
                {
                    entriesforClassification[competition].Add(category, new List<RankingEntry> { entry });
                }
                else
                {
                    entriesforClassification.Add(
                        competition,
                        new Dictionary<AthleteCategory, List<RankingEntry>>
                        { 
                            { category, new List<RankingEntry> { entry } } 
                        });
                }

            }
        }
        foreach (var (competition, entriesByCategory) in entriesforClassification)
        {
            foreach (var (category, entries) in entriesByCategory)
            {
                result.Add(new Ranking(competition.Name, category, entries));
            }
        }
        return result;
    }

    private AthleteCategory EmsCategoryToAthleteCategory(EmsCategory category)
    {
        return category switch
        {
            EmsCategory.Seniors => AthleteCategory.Senior,
            EmsCategory.Children => AthleteCategory.Children,
            EmsCategory.JuniorOrYoungAdults => AthleteCategory.JuniorOrYoundAdult,
            _ => throw new NotImplementedException(),
        };
    }
}

public interface IEmsToCoreImporter : ITransientService
{
    Task Import(string filePath, bool adjustTime = true);
}