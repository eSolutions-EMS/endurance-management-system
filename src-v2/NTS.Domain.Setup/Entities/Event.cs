using NTS.Domain.Setup.Import;
using Newtonsoft.Json;
using NTS.Domain.Extensions;
using Not.Reflection;

namespace NTS.Domain.Setup.Entities;

public class Event : DomainEntity, ISummarizable, IImportable, IParent<Official>, IParent<Competition> 
{
    public static Event Create(string place, Country country) => new(place, country);
    public static Event Update(int id, string place, Country country, IEnumerable<Competition> competitions, IEnumerable<Official> officials)
        => new(id, place, country, competitions, officials);

    private List<Competition> _competitions = new();
    private List<Official> _officials = new();

    [JsonConstructor]
    private Event(int id, string place, Country country, IEnumerable<Competition> competitions, IEnumerable<Official> officials) : this(place, country)
    {
        Id = id;
        _competitions = (competitions ?? Enumerable.Empty<Competition>()).ToList();
        _officials = (officials ?? Enumerable.Empty<Official>()).ToList();
    }
    private Event(string place, Country country)
    {
        if (string.IsNullOrEmpty(place) || !char.IsUpper(place.First()))
        {
            throw new DomainException(nameof(Place), $"{nameof(Place)} is invalid. It has to be Capitalized");
        }

        Place = place;
        Country = country;
    }

    public string Place { get; private set; }
    public Country Country { get; private set; }
    public IReadOnlyList<Official> Officials
    {
        get => _officials.AsReadOnly();
        private set => _officials = value.ToList();
    }
    public IReadOnlyList<Competition> Competitions
    {
        get => _competitions.AsReadOnly();
        private set => _competitions = value.ToList();
    }
    public void Add(Competition competition)
    {
        _competitions.Add(competition);
    }
    public void Update(Competition competition)
    {
        _competitions.Update(competition);
    }
    public void Remove(Competition competition)
    {
        _competitions.Remove(competition);
    }
    public void Add(Official official)
    {
        ThrowIfInvalidRole(official);

        _officials.Add(official);
    }
    public void Update(Official official)
    {
        // TODO: fix check for roles
        _officials.Update(official);
    }
    public void Remove(Official official)
    {
        _officials.Remove(official);
    }

    public string Summarize()
	{
		var summary = new Summarizer(this);
        summary.Add(nameof(this.Officials).Localize(), this.Officials);
        summary.Add(nameof(this.Competitions).Localize(), this.Competitions);
        return summary.ToString();
	}
	public override string ToString()
	{
        return $"{this.Place}, {this.Country}";
	}

    private void ThrowIfInvalidRole(Official member)
    {
        var role = member.Role;
        if (member.IsUniqueRole())
        {
            var existing = _officials.FirstOrDefault(x => x.Role == role);
            if (existing != null && existing != member)
            {
                throw new DomainException("Official '{0}' already exists", member.Role.GetDescription());
            }
        }
    }
}
