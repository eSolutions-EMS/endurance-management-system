using Not.Events;

namespace NTS.Domain.Core.Events;

public record QualificationRestored : DomainObject, IEvent
{
    public QualificationRestored(int Number)
    {
        this.Number = Number;
    }

    public int Number { get; }
}
