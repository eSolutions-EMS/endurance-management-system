using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Objects;

public record HandoutDocument : Document
{
    public HandoutDocument(DocumentHeader header, Tandem tandem, PhaseCollection phases) : base(header)
    {
        Tandem = tandem;
        Phases = phases;
    }

    public Tandem Tandem { get; }
    public PhaseCollection Phases { get; }
}
