using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Exceptions;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Enums;
using NTS.Judge.Blazor.Pages.Dashboard.Components.Actions.EliminationForms;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class ParticipationBehind : ObservableBehind, IParticipationBehind
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
    public Participation? SelectedParticipation { get; set; }

    // TODO: we need a better solution to load items as they have been changed in addition to load on startup.
    // Example case: importing previous data: as it is currently we have to restart the app after import
    // Maybe some sort of observable repositories?
    public override async Task Initialize()
    {
        Participations = await _participationRepository.ReadAll();
        SelectedParticipation = Participations.FirstOrDefault();
    }

    public void SelectParticipation(int number)
    {
        SelectedParticipation = Participations.FirstOrDefault(x => x.Tandem.Number == number);
        GuardHelper.ThrowIfDefault(SelectedParticipation);
        
        EmitChange();
    }

    public void RequestReinspection(bool requestFlag)
    {
        SelectedParticipation!.RequestReinspection(requestFlag);
        _participationRepository.Update(SelectedParticipation);

        EmitChange();
    }

    public void RequestRequiredInspection(bool requestFlag)
    {
        SelectedParticipation!.RequestRequiredInspection(requestFlag);
        _participationRepository.Update(SelectedParticipation);

        EmitChange();
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

    public async Task Withdraw()
    {
        GuardHelper.ThrowIfDefault(SelectedParticipation);
        SelectedParticipation.Withdraw();
        await _participationRepository.Update(SelectedParticipation);

        EmitChange();
    }

    public async Task Retire()
    {
        GuardHelper.ThrowIfDefault(SelectedParticipation);
        SelectedParticipation.Retire();
        await _participationRepository.Update(SelectedParticipation);

        EmitChange();
    }

    public async Task FinishNotRanked(string reason)
    {
        GuardHelper.ThrowIfDefault(SelectedParticipation);
        SelectedParticipation.FinishNotRanked(reason);
        await _participationRepository.Update(SelectedParticipation);

        EmitChange();
    }

    public async Task Disqualify(string reason)
    {
        GuardHelper.ThrowIfDefault(SelectedParticipation);
        SelectedParticipation.Disqualify(reason);
        await _participationRepository.Update(SelectedParticipation);

        EmitChange();
    }

    public async Task FailToQualify(string? reason, params FTQCodes[] ftqCodes)
    {
        GuardHelper.ThrowIfDefault(SelectedParticipation);
        GuardHelper.ThrowIfDefault(ftqCodes);
        SelectedParticipation.FailToQualify(reason, ftqCodes);
        await _participationRepository.Update(SelectedParticipation);

        EmitChange();
    }

    public async Task RestoreQualification()
    {
        GuardHelper.ThrowIfDefault(SelectedParticipation);
        SelectedParticipation.RestoreQualification();
        await _participationRepository.Update(SelectedParticipation);

        EmitChange();
    }

    public async Task<Participation?> Get(int id)
    {
        return await _participationRepository.Read(id);
    }
}