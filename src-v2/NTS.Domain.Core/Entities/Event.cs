using Newtonsoft.Json;
using Not.Domain;
using NTS.Domain.Objects;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Entities;

public class Event : DomainEntity
{
    [JsonConstructor]
    public Event(string name, Country country, string place, string? feiId, string? feiCode, string? showFeiId, IEnumerable<Official> officials)
    {
        Name = name;
        Country = country;
        Place = place;
        FeiId = feiId;
        FeiCode = feiCode;
        ShowFeiId = showFeiId;
        Officials = new(officials.ToList());
    }

    public string Name { get; }
    public Country Country { get; }
    public string Place { get; }
    public string? FeiId { get; }
    public string? FeiCode { get; }
    public string? ShowFeiId { get; }
    public ReadOnlyCollection<Official> Officials { get; }

    public override string ToString()
    {
        return $"{Name} {Place}, {Country}";
    }
}
