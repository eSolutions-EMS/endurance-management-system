using NTS.Domain.Setup.Import;
using Newtonsoft.Json;
using NTS.Domain.Extensions;

namespace NTS.Domain.Setup.Entities;

public class Event : DomainEntity, ISummarizable, IImportable, IParent<Phase>, IParent<Official>, IParent<Competition> 
{
    public static Event Create(string place, Country country) => new(place, country);
    public static Event Update(int id, string place, Country country, IEnumerable<Phase> phases, IEnumerable<Competition> competitions, IEnumerable<Official> officials)
        => new(id, place, country, phases, competitions, officials);

    private List<Phase> _phases = new();
    private List<Competition> _competitions = new();
    private List<Official> _officials = new();

    [JsonConstructor]
    private Event(int id, string place, Country country, IEnumerable<Phase> phases, IEnumerable<Competition> competitions, IEnumerable<Official> officials) : this(place, country)
    {
        Id = id;
        _phases = phases.ToList();
        _competitions = competitions.ToList();
        _officials = officials.ToList();
    }
    private Event(string place, Country country)
    {
        if (string.IsNullOrEmpty(place) || !char.IsUpper(place.First()))
        {
            throw new DomainException(nameof(Place), $"{nameof(Place)} is invalid. It has to be Capitalized");
        }

        this.Place = place;
        this.Country = country;
    }

    public string Place { get; private set; }
    public Country Country { get; private set; }
    public IReadOnlyList<Phase> Phases
    {
        get => _phases.AsReadOnly();
        private set => _phases = value.ToList();
    }
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
    public void Add(Phase phase)
    {
        _phases.Add(phase);
    }

    public void Remove(Phase phase)
    {
        _phases.Remove(phase);
    }

    public void Update(Phase phase)
    {
        _phases.Update(phase);
    }
    public void Add(Competition competition)
    {
        this._competitions.Add(competition);
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
