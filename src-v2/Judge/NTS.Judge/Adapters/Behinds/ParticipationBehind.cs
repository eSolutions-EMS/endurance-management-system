using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
using Not.Startup;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Enums;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class ParticipationBehind : ObservableBehind, IParticipationBehind, IStartupInitializerAsync
{
    private readonly IRepository<Participation> _participationRepository;
    private readonly IRepository<SnapshotResult> _snapshotResultRepository;
    
    public ParticipationBehind(
        IRepository<Participation> participationRepository,
        IRepository<SnapshotResult> snapshotResultRepository)
    {
        _participationRepository = participationRepository;
        _snapshotResultRepository = snapshotResultRepository;
    }

    public IEnumerable<Participation> Participations { get; private set; } = new List<Participation>();
    public IEnumerable<IGrouping<double, Participation>> ParticipationsByDistance => Participations.GroupBy(x => x.Phases.Distance);

    // TODO: we need a better solution to load items as they have been changed in addition to load on startup.
    // Example case: importing previous data: as it is currently we have to restart the app after import
    // Maybe some sort of observable repositories?
    public async Task RunAtStartup()
    {
        Participations = await _participationRepository.ReadAll();
    }

    public async Task Process(Snapshot snapshot)
    {
        var participation = Participations.FirstOrDefault(x => x.Tandem.Number == snapshot.Number);
        GuardHelper.ThrowIfDefault(participation);

        var result = participation.Process(snapshot);
        if (result.Type == SnapshotResultType.Applied)
        {
            await _participationRepository.Update(participation);
        }
        await _snapshotResultRepository.Create(result);

        EmitChange();
    }

    public async Task Update(IPhaseState state)
    {
        var participation = await _participationRepository.Read(x => x.Phases.Any(y => y.Id == state.Id));
        GuardHelper.ThrowIfDefault(participation);

        participation.Update(state);
        await _participationRepository.Update(participation);
    }

    public async Task RevokeQualification(int number, QualificationRevokeType type, FTQCodes? ftqCode = null, string? reason = null)
    {
        GuardHelper.ThrowIfDefault(type);

        var participation = await _participationRepository.Read(x => x.Tandem.Number == number);
        GuardHelper.ThrowIfDefault(participation);

        if (type == QualificationRevokeType.Withdraw)
        {
            participation.Withdraw();
        }
        if (type == QualificationRevokeType.Retire)
        {
            participation.Retire();
        }
        if (type == QualificationRevokeType.Disqualify)
        {
            participation.Disqualify(reason);
        }
        if (type == QualificationRevokeType.FinishNotRanked)
        {
            participation.FinishNotRanked(reason);
        }
        if (type == QualificationRevokeType.FailToQualify)
        {
            GuardHelper.ThrowIfDefault(ftqCode);
            participation.FailToQualify(ftqCode.Value);
        }
        if (type == QualificationRevokeType.FailToCompleteLoop)
        {
            participation.FailToCompleteLoop(reason);
        }

        await _participationRepository.Update(participation);
    }

    public async Task RestoreQualification(int number)
    {
        var participation = await _participationRepository.Read(x => x.Tandem.Number == number);
        GuardHelper.ThrowIfDefault(participation);

        participation.RestoreQualification();
        await _participationRepository.Update(participation);
    }
}