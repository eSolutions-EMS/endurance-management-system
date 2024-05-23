using Not.Application.Ports.CRUD;
using Not.Events;
using Not.Startup;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Events;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.Ports;

namespace NTS.Judge.Services;

public class CoreEventInitializer : IInitializer
{
    private readonly IDocumentBehind _documentBehind;
    private readonly IParticipationBehind _participationBehind;
    private readonly IRemoteProcedures _rpcHub;
    private readonly IDocumentHandler _documentHandler;

    public CoreEventInitializer(
        IDocumentBehind documentBehind,
        IParticipationBehind participationBehind,
        IRemoteProcedures rpcHub,
        IDocumentHandler documentHandler)
    {
        _documentBehind = documentBehind;
        _participationBehind = participationBehind;
        _rpcHub = rpcHub;
        _documentHandler = documentHandler;
    }

    public void Run()
    {
        EventHelper.Subscribe<PhaseCompleted>(OnPhaseCompleted);
        EventHelper.Subscribe<StartCreated>(OnStartCreated);
        EventHelper.Subscribe<QualificationRevoked>(OnQualificationRevoked);
        EventHelper.Subscribe<QualificationRestored>(OnQualificationRestored);
        EventHelper.Subscribe<DocumentProduced>(OnDocumentProduced);
    }

    public async void OnPhaseCompleted(PhaseCompleted phaseCompleted)
    {
        // TODO: Probably remove this and send StartCreated directly as it is
        // kind of pointless to trigger and handle event in the same class
        await _participationBehind.CreateStart(phaseCompleted.Number);
        await _documentBehind.CreateHandout(phaseCompleted.Number);
    }

    public void OnStartCreated(StartCreated startCreated)
    {
        _rpcHub.SendStartCreated(startCreated);
    }

    private void OnQualificationRevoked(QualificationRevoked revoked)
    {
        _rpcHub.SendQualificationRevoked(revoked);
    }

    private void OnQualificationRestored(QualificationRestored restored)
    {
        _rpcHub.SendQualificationRestored(restored);
    }

    private void OnDocumentProduced(DocumentProduced produced)
    {
        _documentHandler.Handle(produced.Document);
    }
}
