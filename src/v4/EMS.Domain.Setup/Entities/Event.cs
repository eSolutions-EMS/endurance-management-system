using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Event : DomainEntity, ISummarizable, IImportable, IParent<Official>
{
    private List<Competition> _competitions = new();
    private List<Official> _officials = new();

    public Event(int id, string place, Country country) : this(place, country)
    {
        Id = id;
    }
    public Event(string place, Country country)
    {
        this.Place = place;
        this.Country = country;
    }

    public string Place { get; }
    public Country Country { get; }
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
	
    public void Add(Official staffMember)
    {
        this.ThrowIfInvalidRole(staffMember);

        this._officials.Add(staffMember);
    }
    public void Update(Official child)
    {
        this._officials.Remove(child);

        this.Add(child);
    }
    public void Remove(Official staffMember)
    {
        this._officials.Remove(staffMember);
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
                throw new DomainException("Staff  member '", member.Role, "' already exists");
            }
        }
    }
}
