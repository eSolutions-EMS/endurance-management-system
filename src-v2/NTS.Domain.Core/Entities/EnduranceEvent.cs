using Newtonsoft.Json;
using NTS.Domain.Core.Objects;

namespace NTS.Domain.Core.Entities;

public class EnduranceEvent : DomainEntity
{
    [JsonConstructor]
    private EnduranceEvent(
        int id,
        PopulatedPlace populatedPlace,
        EventSpan eventSpan,
        string? feiId,
        string? feiCode,
        string? showFeiId
    )
        : base(id)
    {
        PopulatedPlace = populatedPlace;
        EventSpan = eventSpan;
        FeiId = feiId;
        FeiCode = feiCode;
        ShowFeiId = showFeiId;
    }

    public EnduranceEvent(
        Country country,
        string city,
        string place,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string? feiId,
        string? feiCode,
        string? showFeiId
    )
        : this(
            GenerateId(),
            new PopulatedPlace(country, city, place),
            new EventSpan(startDate, endDate),
            feiId,
            feiCode,
            showFeiId
        ) { }

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
