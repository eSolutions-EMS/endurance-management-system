using Newtonsoft.Json;

namespace NTS.Domain.Core.Entities.ParticipationAggregate;

public class NotQualified : DomainEntity
{
    [JsonConstructor]
    public NotQualified(NotQualifiedType type, string code)
    {
        Type = type;
        Code = code;
    }

    public NotQualifiedType Type { get; }
    public string Code { get; }
}

public enum NotQualifiedType
{
    FTQ = 0,
    DSQ = 1,
    RET = 2,
}
