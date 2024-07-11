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

namespace NTS.Judge.ACL;

public class EmsToCoreImport
{
    private readonly IRepository<Event> _events;
    private readonly IRepository<Official> _officials;
    private readonly IRepository<Participation> _participations;
    private readonly IRepository<Classification> _classfications;

    public EmsToCoreImport(
        IRepository<Event> events,
        IRepository<Official> officials,
        IRepository<Participation> participations,
        IRepository<Classification> classfications)
    {
        _events = events;
        _officials = officials;
        _participations = participations;
        _classfications = classfications;
    }

    public async Task Import(string filePath)
    {
        var contents = await File.ReadAllTextAsync(filePath);
        var emsState = contents.FromJson<EmsState>();

        var @event = CreateEvent(emsState.Event);
        var officials = CreateOfficials(emsState.Event);
        var participations = CreateParticipations(emsState);
        var classifications = CreateClassifications(emsState);

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

    private Event CreateEvent(EmsEnduranceEvent emsEvent)
    {
        var country = new Country(emsEvent.Country.IsoCode, emsEvent.Country.Name);
        var startTime = emsEvent.Competitions.OrderBy(x => x.StartTime).First().StartTime;
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
        var result = new List<Official>
        {
            new (new Person(emsEvent.PresidentGroundJury.Name), PresidentGroundJury),
            new (new Person(emsEvent.PresidentVetCommittee.Name), PresidentVet),
            new (new Person(emsEvent.FeiTechDelegate.Name), FeiTechDelegate),
            new (new Person(emsEvent.FeiVetDelegate.Name), FeiVetDelegate),
            new (new Person(emsEvent.ForeignJudge.Name), ForeignJudge),
            new (new Person(emsEvent.ActiveVet.Name), ActiveVet),
        };
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

    private IEnumerable<Participation> CreateParticipations(EmsState state)
    {
        foreach (var emsParticipation in state.Participations)
        {
            foreach (var competitionId in emsParticipation.CompetitionsIds)
            {
                var competition = state.Event.Competitions.First(x => x.Id == competitionId);
                yield return ParticipationFactory.CreateCore(emsParticipation, competition);
            }
        }
    }

    private IEnumerable<Classification> CreateClassifications(EmsState state)
    {
        var entriesforClassification = new Dictionary<EmsCompetition, Dictionary<AthleteCategory, List<ClassificationEntry>>>();
        foreach (var emsParticipation in state.Participations)
        {
            foreach (var competitionId in emsParticipation.CompetitionsIds)
            {
                var competition = state.Event.Competitions.First(x => x.Id == competitionId);
                var participation = ParticipationFactory.CreateCore(emsParticipation, competition);
                var category = EmsCategoryToAthleteCategory(emsParticipation.Participant.Athlete.Category);
                var entry = new ClassificationEntry(participation, emsParticipation.Participant.Unranked);
                if (entriesforClassification.ContainsKey(competition) && entriesforClassification[competition].ContainsKey(category))
                {
                    entriesforClassification[competition][category].Add(entry);
                }
                else
                {
                    entriesforClassification[competition][category] = new List<ClassificationEntry> { entry };
                }

            }
        }
        foreach (var (competition, entriesByCategory) in entriesforClassification)
        {
            foreach (var (category, entries) in entriesByCategory)
            {
                yield return new Classification(competition.Name, category, entries);
            }
        }
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
