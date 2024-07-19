using Not.Events;
using NTS.Domain.Core.Abstractions;

namespace NTS.Domain.Core.Events;

public record QualificationRestored : DomainObject, IEvent, IParticipationIdentifier
{
    public QualificationRestored(int Number)
    {
        this.Number = Number;
    }

    public int Number { get; }
}
