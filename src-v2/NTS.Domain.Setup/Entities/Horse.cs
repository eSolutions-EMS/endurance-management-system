using Newtonsoft.Json;
using Not.Domain.Base;

namespace NTS.Domain.Setup.Entities;

public class Horse : DomainEntity, IAggregateRoot
{
    public static Horse Create(string? name, string? feiId)
    {
        return new(name, feiId);
    }

    public static Horse Update(int id, string? name, string? feiId)
    {
        return new(id, name, feiId);
    }

    [JsonConstructor]
    Horse(int id, string? name, string? feiId)
        : base(id)
    {
        Name = Required(nameof(Name), name);
        FeiId = feiId;
    }

    Horse(string? name, string? feiId)
        : this(GenerateId(), name, feiId) { }

    public string? FeiId { get; }
    public string Name { get; }

    public string Summarize()
    {
        return ToString();
    }

    public override string ToString()
    {
        return Name;
    }
}
