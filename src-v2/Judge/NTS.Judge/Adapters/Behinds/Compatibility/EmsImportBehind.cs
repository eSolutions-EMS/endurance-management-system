using Not.Application.CRUD.Ports;
using Not.Extensions;
using Not.Safe;
using Not.Serialization;
using NTS.Compatibility.EMS;
using NTS.Compatibility.EMS.Entities.EnduranceEvents;
using NTS.Domain.Enums;
using NTS.Domain.Objects;
using NTS.Domain.Setup.Entities;
using NTS.Judge.ACL;
using NTS.Judge.Blazor.Ports;
using static NTS.Domain.Enums.OfficialRole;

namespace NTS.Judge.Adapters.Behinds.Compatibility;

public class EmsImportBehind : IEmsImportBehind
{
    readonly IRepository<EnduranceEvent> _eventRepository;
    readonly IEmsToCoreImporter _emsToCoreImporter;

    public EmsImportBehind(
        IRepository<EnduranceEvent> eventRepository,
        IEmsToCoreImporter emsToCoreImporter
    )
    {
        _eventRepository = eventRepository;
        _emsToCoreImporter = emsToCoreImporter;
    }

    public Task Import(string path)
    {
        Task action() => SafeImport(path);
        return SafeHelper.Run(action);
    }

    public Task ImportCore(string contents)
    {
        Task action() => SafeImportCore(contents);
        return SafeHelper.Run(action);
    }

    async Task SafeImport(string emsStateFilePath)
    {
        var contents = await File.ReadAllTextAsync(emsStateFilePath);
        var emsState = contents.FromJson<EmsState>();

        var country = new Country(
            emsState.Event.Country.IsoCode,
            "zz",
            emsState.Event.Country.Name
        );
        var enduranceEvent = EnduranceEvent.Create(emsState.Event.PopulatedPlace, country);

        foreach (var offical in CreateOfficials(emsState.Event))
        {
            enduranceEvent.Add(offical);
        }
        foreach (var competition in CreateCompetitions(emsState.Event))
        {
            enduranceEvent.Add(competition);
        }

        await _eventRepository.Update(enduranceEvent);
    }

    async Task SafeImportCore(string contents)
    {
        await _emsToCoreImporter.Import(contents);
    }

    IEnumerable<Competition> CreateCompetitions(EmsEnduranceEvent emsEvent)
    {
        foreach (var emsCompetition in emsEvent.Competitions)
        {
            var (type, ruleset) = MapRuleset(emsCompetition.Type);
            var start = emsCompetition.StartTime.ToDateTimeOffset();
            yield return Competition.Create(emsCompetition.Name, type, ruleset, start, 10);
        }

        static (CompetitionType type, CompetitionRuleset ruleset) MapRuleset(
            NTS.Compatibility.EMS.Entities.Competitions.EmsCompetitionType emsType
        )
        {
            if (emsType == NTS.Compatibility.EMS.Entities.Competitions.EmsCompetitionType.National)
            {
                return (CompetitionType.Qualification, CompetitionRuleset.Regional);
            }
            else if (
                emsType
                == NTS.Compatibility.EMS.Entities.Competitions.EmsCompetitionType.International
            )
            {
                return (CompetitionType.Star, CompetitionRuleset.FEI);
            }
            throw new Exception();
        }
    }

    IEnumerable<Official> CreateOfficials(EmsEnduranceEvent emsEvent)
    {
        var result = new List<Official>
        {
            Official.Create(emsEvent.PresidentGroundJury.Name, GroundJuryPresident),
            Official.Create(emsEvent.PresidentVetCommittee.Name, VeterinaryCommissionPresident),
            Official.Create(emsEvent.FeiTechDelegate.Name, TechnicalDelegate),
            Official.Create(emsEvent.FeiVetDelegate.Name, ForeignVeterinaryDelegate),
            Official.Create(emsEvent.ForeignJudge.Name, ForeignJudge),
        };
        foreach (var jury in emsEvent.MembersOfJudgeCommittee)
        {
            var item = Official.Create(jury.Name, GroundJury);
            result.Add(item);
        }
        ;
        foreach (var vet in emsEvent.MembersOfVetCommittee)
        {
            var item = Official.Create(vet.Name, VeterinaryCommission);
            result.Add(item);
        }
        return result;
    }
}
