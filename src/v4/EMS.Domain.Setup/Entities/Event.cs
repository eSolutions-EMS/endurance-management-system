using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Event : DomainEntity, ISummarizable, IImportable, IParent<StaffMember>
{
    private List<Competition> competitionList = new();
    private List<StaffMember> personnelList = new();

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
    public IReadOnlyList<StaffMember> Staff
    {
        get => personnelList.AsReadOnly();
        private set => personnelList = value.ToList();
    }
    public IReadOnlyList<Competition> Competitions
    {
        get => competitionList.AsReadOnly();
        private set => competitionList = value.ToList();
    }
	
    public void Add(StaffMember staffMember)
    {
        this.ThrowIfInvalidRole(staffMember);

        this.personnelList.Add(staffMember);
    }
    public void Update(StaffMember child)
    {
        this.personnelList.Remove(child);

        this.Add(child);
    }
    public void Remove(StaffMember staffMember)
    {
        this.personnelList.Remove(staffMember);
    }

    public string Summarize()
	{
		var summary = new Summarizer(this);
        summary.Add(nameof(this.Staff).Localize(), this.Staff);
        summary.Add(nameof(this.Competitions).Localize(), this.Competitions);
        return summary.ToString();
	}
	public override string ToString()
	{
        return $"{this.Place}, {this.Country}";
	}

    private void ThrowIfInvalidRole(StaffMember member)
    {
        var role = member.Role;
        if (role == StaffRole.PresidentVet ||
            role == StaffRole.PresidentGroundJury ||
            role == StaffRole.ActiveVet ||
            role == StaffRole.FeiDelegateVet ||
            role == StaffRole.FeiDelegateTech ||
            role == StaffRole.ForeignJudge)
        {
            var existing = personnelList.FirstOrDefault(x => x.Role == role);
            if (existing != null && existing != member)
            {
                throw new DomainException("Staff  member '", member.Role, "' already exists");
            }
        }
    }
}
