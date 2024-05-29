using Newtonsoft.Json;
using NTS.Domain.Core.Objects;

namespace NTS.Domain.Core.Entities;

public class Event : DomainEntity
{
    [JsonConstructor]
    public Event(
        Country country,
        string city,
        string place,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string? feiId,
        string? feiCode, 
        string? showFeiId)
    {
        PopulatedPlace = new PopulatedPlace(country, city, place);
        EventSpan = new EventSpan(startDate, endDate);
        FeiId = feiId;
        FeiCode = feiCode;
        ShowFeiId = showFeiId;
    }

    public PopulatedPlace PopulatedPlace { get; }
    public EventSpan EventSpan { get; }
    public string? FeiId { get; }
    public string? FeiCode { get; }
    public string? ShowFeiId { get; }

    public override string ToString()
    {
        return $"{PopulatedPlace} {EventSpan}";
    }
}
