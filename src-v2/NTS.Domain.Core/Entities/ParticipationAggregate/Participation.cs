using System.Collections.ObjectModel;
using static NTS.Domain.Enums.SnapshotType;

namespace NTS.Domain.Core.Entities.ParticipationAggregate;

public class Participation : DomainEntity, IAggregateRoot
{
    public Participation(Tandem tandem, IEnumerable<Phase> phases)
    {
        Tandem = tandem;
        Phases = new(phases.ToList());
    }

    public Tandem Tandem { get; private set; }
    public ReadOnlyCollection<Phase> Phases { get; private set; }
    public NotQualified? notQualified { get; private set; }
    public Total? Total => Phases.Any(x => x.IsComplete)
        ? new Total(Phases.Where(x => x.IsComplete))
        : default;
}
