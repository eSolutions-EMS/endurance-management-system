﻿using Not.Application.CRUD.Ports;
using Not.Domain.Exceptions;
using Not.Safe;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Entities.ParticipationAggregate;
using NTS.Domain.Enums;
using NTS.Judge.ACL.Adapters;
using NTS.Judge.Blazor.Core.Ports;

namespace NTS.Judge.Core.Behinds.Adapters;

public class CoreBehind : ICoreBehind
{
    readonly IEmsImporter _emsImporter;
    readonly IRepository<Domain.Setup.Entities.EnduranceEvent> _setupRepository;
    readonly IRepository<EnduranceEvent> _coreEventRespository;
    readonly IRepository<Official> _coreOfficialRepository;
    readonly IRepository<Participation> _participationRepository;
    readonly IRepository<Ranking> _rankingRepository;

    public CoreBehind(
        IEmsImporter emsImporter,
        IRepository<Domain.Setup.Entities.EnduranceEvent> setupRepository,
        IRepository<EnduranceEvent> coreEventRespository,
        IRepository<Official> coreOfficialRepository,
        IRepository<Participation> participationRepository,
        IRepository<Ranking> rankingRepository
    )
    {
        _emsImporter = emsImporter;
        _setupRepository = setupRepository;
        _coreEventRespository = coreEventRespository;
        _coreOfficialRepository = coreOfficialRepository;
        _participationRepository = participationRepository;
        _rankingRepository = rankingRepository;
    }

    public Task Start()
    {
        return SafeHelper.Run(SafeStart);
    }

    public Task<bool> IsEnduranceEventStarted()
    {
        return SafeHelper.Run(SafeIsEnduranceEventStarted);
    }

    public Task<bool> IsStarted()
    {
        return IsEnduranceEventStarted();
    }

    public Task Import(string contents)
    {
        return _emsImporter.ImportCore(contents);
    }

    async Task SafeStart()
    {
        var setupEvent = await _setupRepository.Read(0);
        if (setupEvent == null)
        {
            // TODO: Create ValidationException containing localization logic and inherit form it in DomainException. Use that here instead
            throw new DomainException("Cannot start - Event is not configured");
        }
        await CreateEvent(setupEvent);
        await CreateOfficials(setupEvent.Officials);
        await CreateParticipationsAndRankings(setupEvent);
    }

    async Task<bool> SafeIsEnduranceEventStarted()
    {
        var a = await _coreEventRespository.Read(0);
        return a != null;
    }

    async Task CreateEvent(Domain.Setup.Entities.EnduranceEvent setupEvent)
    {
        var @event = CoreFactory.CreateEvent(setupEvent);
        await _coreEventRespository.Create(@event);
    }

    async Task CreateOfficials(IEnumerable<Domain.Setup.Entities.Official> setupOfficials)
    {
        foreach (var setupOfficial in setupOfficials)
        {
            var official = CoreFactory.CreateOfficial(setupOfficial);
            await _coreOfficialRepository.Create(official);
        }
    }

    async Task CreateParticipationsAndRankings(Domain.Setup.Entities.EnduranceEvent setupEvent)
    {
        foreach (var competition in setupEvent.Competitions)
        {
            var (participations, rankingEntriesByCategory) =
                await CoreFactory.CreateParticipationAndRankingEntriesAsync(
                    competition,
                    _participationRepository
                );
            foreach (var participation in participations)
            {
                await _participationRepository.Create(participation);
            }
            await CreateRankings(
                new Competition(competition.Name, competition.Ruleset),
                rankingEntriesByCategory
            );
        }
    }

    async Task CreateRankings(
        Competition competition,
        Dictionary<AthleteCategory, List<RankingEntry>> rankingEntriesByCategory
    )
    {
        foreach (var relation in rankingEntriesByCategory)
        {
            var ranking = CoreFactory.CreateRanking(competition, relation.Key, relation.Value);
            await _rankingRepository.Create(ranking);
        }
    }
}