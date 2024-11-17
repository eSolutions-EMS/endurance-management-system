using Newtonsoft.Json;
using Not.Domain.Base;

namespace NTS.Domain.Core.Entities;

public class Handout : DomainEntity, IAggregateRoot
{
    [JsonConstructor]
    Handout(int id, Participation participation)
        : base(id)
    {
        Participation = participation;
    }

    public Handout(Participation participation)
        : this(GenerateId(), participation) { }

    public Participation Participation { get; }
}
