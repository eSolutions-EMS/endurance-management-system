using Not.Application.Ports.CRUD;
using Not.Events;
using Not.Startup;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Events;
using NTS.Domain.Core.Services;

namespace NTS.Judge.Services;

public class CoreEventInitializer : IInitializer
{
    private readonly IRepository<Participation> _participationRepository;

    public CoreEventInitializer(IRepository<Participation> participationRepository)
    {
        _participationRepository = participationRepository;
    }

    public void Run()
    {
        EventHelper.Subscribe<SnapshotReceived>(OnSnapshotReceived);
        EventHelper.Subscribe<PhaseCompleted>(OnPhaseCompleted);
        EventHelper.Subscribe<StartCreated>(OnStartCreated);
        EventHelper.Subscribe<QualificationRevoked>(OnQualificationRevoked);
        EventHelper.Subscribe<QualificationRestored>(OnQualificationRestored);
        EventHelper.Subscribe<DocumentProduced>(OnDocumentProduced);
    }

    public async void OnSnapshotReceived(SnapshotReceived snapshotReceived)
    {
        var participation = await GetParticipation(snapshotReceived.Number);
        participation.Process(snapshotReceived.Snapshot);
    }

    public async void OnPhaseCompleted(PhaseCompleted phaseCompleted)
    {
        var participation = await GetParticipation(phaseCompleted.TandemNumber);
        StartProducer.CreateStart(participation);
    }

    public void OnStartCreated(StartCreated startCreated)
    {
        throw new NotImplementedException();
    }

    private void OnQualificationRestored(QualificationRestored restored)
    {
        throw new NotImplementedException();
    }

    private void OnQualificationRevoked(QualificationRevoked revoked)
    {
        throw new NotImplementedException();
    }

    private void OnDocumentProduced(DocumentProduced produced)
    {
        throw new NotImplementedException();
    }

    private async Task<Participation> GetParticipation(int number)
    {
        var participation = await _participationRepository.Read(x => x.Tandem.Number == number);
        if (participation == null)
        {
            //TODO: Create a VerboseWarningException to interrupt the flow and then INotifier would display the message based on Verbose configuration
            throw new Exception($"Participation '{number}' does not exist");
        }
        return participation;
    }
}
