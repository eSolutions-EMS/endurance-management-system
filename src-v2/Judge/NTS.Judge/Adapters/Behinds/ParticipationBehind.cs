using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Events;
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
    public Participation SelectedParticipation { get; private set; } = default!;

    // TODO: we need a better solution to load items as they have been changed in addition to load on startup.
    // Example case: importing previous data: as it is currently we have to restart the app after import
    // Maybe some sort of observable repositories?
    public async Task RunAtStartup()
    {
        Participations = await _participationRepository.ReadAll();
        GuardHelper.ThrowIfDefault(Participations);
        SelectParticipation(null);
    }

    public void SelectParticipation(int? number)
    {
        Participation participation = default!;
        if (number == null)
        {
            participation = Participations.FirstOrDefault();
        }
        else
        {
            participation = Participations.FirstOrDefault(x => x.Tandem.Number == number);
        }
        GuardHelper.ThrowIfDefault(participation);
        SelectedParticipation = participation;
        EventHelper.Emit(SelectedParticipation);
    }

    public async Task Process(Snapshot snapshot)
    {
        if (SelectedParticipation == default)
        {
            SelectParticipation(snapshot.Number);
        }
        GuardHelper.ThrowIfDefault(SelectedParticipation);

        var result = SelectedParticipation?.Process(snapshot);
        if (result.Type == SnapshotResultType.Applied)
        {
            await _participationRepository.Update(SelectedParticipation);
        }
        await _snapshotResultRepository.Create(result);

        EmitChange();
    }

    public async Task Update(IPhaseState state)
    {
        //probably should use Participations collection to ensure a single point of truth is used for the data
        var participation = await _participationRepository.Read(x => x.Phases.Any(y => y.Id == state.Id));
        GuardHelper.ThrowIfDefault(participation);

        participation.Update(state);
        await _participationRepository.Update(participation);
    }

    public async Task Update(Participation participation)
    {
        await _participationRepository.Update(participation);
    }

    public async Task RevokeQualification(int number, QualificationRevokeType type, string? reason = null, params FTQCodes[] ftqCodes)
    {
        GuardHelper.ThrowIfDefault(type);

        var participation = Participations.FirstOrDefault(x => x.Tandem.Number == number);
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
            GuardHelper.ThrowIfDefault(ftqCodes);
            participation.FailToQualify(ftqCodes);
        }
        if (type == QualificationRevokeType.FailToCompleteLoop)
        {
            participation.FailToCompleteLoop(reason);
        }
        await Update(participation);
    }

    public async Task RestoreQualification(int number)
    {
        var participation = Participations.FirstOrDefault(x => x.Tandem.Number == number);
        GuardHelper.ThrowIfDefault(participation);

        participation.RestoreQualification();
        await Update(participation);
    }
}