using Not.Application.CRUD.Ports;
using Not.Exceptions;
using Not.Safe;
using NTS.Domain.Core.Aggregates;
using NTS.Domain.Objects;

namespace NTS.Judge.Core.SnapshotProcessor;

public class SnapshotProcessor : ISnapshotProcessor
{
    readonly IRepository<Participation> _participationRepository;
    readonly IRepository<SnapshotResult> _snapshotResultRepository;

    public SnapshotProcessor(IRepository<Participation> participationRepository, IRepository<SnapshotResult> snapshotResultRepository)
    {
        _participationRepository = participationRepository;
        _snapshotResultRepository = snapshotResultRepository;
    }

    public async Task<Participation> Process(Snapshot snapshot)
    {
        IEnumerable<Participation> Participations = await _participationRepository.ReadAll();
        var participation = Participations.FirstOrDefault(x =>
            x.Combination.Number == snapshot.Number
        );
        GuardHelper.ThrowIfDefault(participation);
        var result = participation.Process(snapshot);
        if (result.Type == SnapshotResultType.Applied)
        {
            await _participationRepository.Update(participation);
        }
        await _snapshotResultRepository.Create(result);
        return participation;
    }
}
