using Not.Application.Ports.CRUD;
using Not.Exceptions;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Services;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class ParticipationBehind : IParticipationBehind
{
    private readonly IRepository<Participation> _participationRepository;

    public ParticipationBehind(IRepository<Participation> participationRepository)
    {
        _participationRepository = participationRepository;
    }

    public async Task Process(Snapshot snapshot)
    {
        var participation = await _participationRepository.Read(x => x.Tandem.Number == snapshot.Number);
        GuardHelper.ThrowIfDefault(participation);

        participation.Process(snapshot);
        await _participationRepository.Update(participation);
    }

    public async Task Update(IPhaseState state)
    {
        var participation = await _participationRepository.Read(x => x.Phases.Any(y => y.Id == state.Id));
        GuardHelper.ThrowIfDefault(participation);

        participation.Update(state);
        await _participationRepository.Update(participation);
    }

    public async Task RevokeQualification(int number, RevokeType type, FTQCodes? ftqCode = null, string? reason = null)
    {
        GuardHelper.ThrowIfDefault(type);

        var participation = await _participationRepository.Read(x => x.Tandem.Number == number);
        GuardHelper.ThrowIfDefault(participation);

        if (type == RevokeType.Withdraw)
        {
            participation.Withdraw();
        }
        if (type == RevokeType.Retire)
        {
            participation.Retire();
        }
        if (type == RevokeType.Dsiqualify)
        {
            participation.Disqualify(reason);
        }
        if (type == RevokeType.FinishNotRanked)
        {
            participation.FinishNotRanked(reason);
        }
        if (type == RevokeType.FailToQualify)
        {
            GuardHelper.ThrowIfDefault(ftqCode);
            participation.FailToQualify(ftqCode.Value);
        }
        if (type == RevokeType.FailToCompleteLoop)
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

    public async Task CreateStart(int number)
    {
        var participation = await _participationRepository.Read(x => x.Tandem.Number == number);
        GuardHelper.ThrowIfDefault(participation);

        StartProducer.CreateStart(participation);
    }
}