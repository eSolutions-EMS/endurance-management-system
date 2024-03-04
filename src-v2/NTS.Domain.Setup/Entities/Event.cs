using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Event : DomainEntity, ISummarizable, IImportable, IParent<Official>
{
    private List<Competition> _competitions = new();
    private List<Official> _officials = new();

    [JsonConstructor]
    public Event(int id, string place, Country country) : this(place, country)
    {
        Id = id;
    }
    public Event(string place, Country country)
    {
        if (string.IsNullOrEmpty(place) || !char.IsUpper(place.First()))
        {
            throw new DomainException(nameof(Place), $"{nameof(Place)} is invalid. It has to be Caplitalized");
        }
        if (country.Name == "Bulgaria")
        {
            throw new DomainException(nameof(Country), "Trolololol");
        }
        this.Place = place;
        this.Country = country;
    }
    public static Event Update(int id, string place, Country country) => new(id, place, country);

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
	
    public void Add(Official official)
    {
        this.ThrowIfInvalidRole(official);

        this._officials.Add(official);
    }
    public void Update(Official official)
    {
        this._officials.Remove(official);

        this.Add(official);
    }
    public void Remove(Official official)
    {
        this._officials.Remove(official);
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
        if (role == OfficialRole.PresidentVet ||
            role == OfficialRole.PresidentGroundJury ||
            role == OfficialRole.ActiveVet ||
            role == OfficialRole.FeiDelegateVet ||
            role == OfficialRole.FeiDelegateTech ||
            role == OfficialRole.ForeignJudge)
        {
            var existing = _officials.FirstOrDefault(x => x.Role == role);
            if (existing != null && existing != member)
            {
                throw new DomainException("Official '", member.Role, "' already exists");
            }
        }
    }
}
