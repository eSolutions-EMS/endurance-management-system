using Not.Events;
using Not.Startup;
using NTS.Domain.Core.Events;
using NTS.Judge.Blazor.Ports;
using NTS.Judge.Ports;

namespace NTS.Judge.Services;

public class CoreEventInitializer : IInitializer
{
    private readonly IDocumentBehind _documentBehind;
    private readonly IRemoteProcedures _rpcHub;
    private readonly IDocumentHandler _documentHandler;

    public CoreEventInitializer(
        IDocumentBehind documentBehind,
        IRemoteProcedures rpcHub,
        IDocumentHandler documentHandler)
    {
        _documentBehind = documentBehind;
        _rpcHub = rpcHub;
        _documentHandler = documentHandler;
    }

    public void Run()
    {
        EventHelper.Subscribe<PhaseCompleted>(OnPhaseCompleted);
        EventHelper.Subscribe<QualificationRevoked>(OnQualificationRevoked);
        EventHelper.Subscribe<QualificationRestored>(OnQualificationRestored);
        EventHelper.Subscribe<DocumentProduced>(OnDocumentProduced);
    }

    public async void OnPhaseCompleted(PhaseCompleted phaseCompleted)
    {
        await _rpcHub.SendStartCreated(phaseCompleted);
        await _documentBehind.CreateHandout(phaseCompleted.Number);
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
