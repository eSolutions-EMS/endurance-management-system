﻿using Not.Application.Ports.CRUD;
using Not.Exceptions;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Services;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class DocumentBehind : IDocumentBehind
{
    private readonly IRepository<Participation> _participationRepository;
    private readonly IRepository<Event> _eventRepository;
    private readonly IRepository<Official> _officialRepository;
    private readonly IRepository<Classification> _classificationRepository;

    public DocumentBehind(
        IRepository<Participation> participationRepository,
        IRepository<Event> eventRepository,
        IRepository<Official> officialRepository,
        IRepository<Classification> classificationRepository)
    {
        _participationRepository = participationRepository;
        _eventRepository = eventRepository;
        _officialRepository = officialRepository;
        _classificationRepository = classificationRepository;
    }

    public async Task CreateHandout(int number)
    {
        var participation = await _participationRepository.Read(x => x.Tandem.Number == number);
        var @event = await _eventRepository.Read(0);
        var officials = await _officialRepository.ReadAll();

        GuardHelper.ThrowIfDefault(@event);
        GuardHelper.ThrowIfDefault(participation);

        DocumentProducer.CreateHandout(@event, officials, participation);
    }

    public async Task CreateRanklist(int classificationId)
    {
        var classification = await _classificationRepository.Read(classificationId);
        var @event = await _eventRepository.Read(0);
        var officials = await _officialRepository.ReadAll();

        GuardHelper.ThrowIfDefault(@event);
        GuardHelper.ThrowIfDefault(classification);

        DocumentProducer.CreateRanklist(@event, officials, classification);
    }
}
