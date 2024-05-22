using Not.Application.Ports.CRUD;
using Not.Extensions;
using Not.Serialization;
using NTS.Compatibility.EMS;
using NTS.Compatibility.EMS.Entities.EnduranceEvents;
using NTS.Domain.Enums;
using NTS.Domain.Objects;
using NTS.Domain.Setup.Entities;
using NTS.Judge.Blazor.Ports;
using static NTS.Domain.Enums.OfficialRole;

namespace NTS.Judge.Adapters.Behinds.Compatibility;

public class EmsImportBehind : IEmsImportBehind
{
    private readonly IRepository<Event> _eventRepository;

    public EmsImportBehind(IRepository<Event> eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Import(string emsStateFilePath)
    {
        var contents = await File.ReadAllTextAsync(emsStateFilePath);
        var emsState = contents.FromJson<EmsState>();

        var country = new Country(emsState.Event.Country.IsoCode, emsState.Event.Country.Name);
        var @event = Event.Create(emsState.Event.PopulatedPlace, country);

        foreach (var offical in CreateOfficials(emsState.Event))
        {
            @event.Add(offical);
        }
        foreach (var competition in CreateCompetitions(emsState.Event))
        {
            @event.Add(@competition);
        }

        await _eventRepository.Update(@event);
    }

    private IEnumerable<Competition> CreateCompetitions(EnduranceEvent emsEvent)
    {
        foreach (var emsCompetition in emsEvent.Competitions)
        {
            yield return Competition.Create(emsCompetition.Name, MapType(emsCompetition.Type), emsCompetition.StartTime.ToDateTimeOffset());
        }

        CompetitionType MapType(NTS.Compatibility.EMS.Entities.Competitions.CompetitionType emsType)
        {
            if (emsType == NTS.Compatibility.EMS.Entities.Competitions.CompetitionType.National)
            {
                return CompetitionType.National;
            }
            else if (emsType == NTS.Compatibility.EMS.Entities.Competitions.CompetitionType.International)
            {
                return CompetitionType.FEI;
            }
            return CompetitionType.Qualification;
        }
    }

    private IEnumerable<Official> CreateOfficials(EnduranceEvent emsEvent)
    {
        var result = new List<Official>
        {
            Official.Create(emsEvent.PresidentGroundJury.Name, PresidentGroundJury),
            Official.Create(emsEvent.PresidentVetCommittee.Name, PresidentVet),
            Official.Create(emsEvent.FeiTechDelegate.Name, FeiTechDelegate),
            Official.Create(emsEvent.FeiVetDelegate.Name, FeiVetDelegate),
            Official.Create(emsEvent.ForeignJudge.Name, ForeignJudge),
            Official.Create(emsEvent.ActiveVet.Name, ActiveVet),
        };
        foreach (var jury in emsEvent.MembersOfJudgeCommittee)
        {
            result.Add(Official.Create(jury.Name, MemberJudge));
        };
        foreach (var vet in emsEvent.MembersOfVetCommittee)
        {
            result.Add(Official.Create(vet.Name, MemberVet));
        }
        return result;
    }
}
