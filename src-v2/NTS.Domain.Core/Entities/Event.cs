using Newtonsoft.Json;

namespace NTS.Domain.Core.Entities;

public class Event : DomainEntity
{
    [JsonConstructor]
    public Event(Country country, string place, string? feiId, string? feiCode, string? showFeiId)
    {
        Country = country;
        Place = place;
        FeiId = feiId;
        FeiCode = feiCode;
        ShowFeiId = showFeiId;
    }

    public Country Country { get; }
    public string Place { get; }
    public string? FeiId { get; }
    public string? FeiCode { get; }
    public string? ShowFeiId { get; }

    public override string ToString()
    {
        return $"{Place}, {Country}";
    }
}
