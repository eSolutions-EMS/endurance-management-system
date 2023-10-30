using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Event : DomainEntity, ISummarizable, IImportable
{
    private readonly List<Competition> competitionList = new();
    private readonly List<StaffMember> personnelList = new();

    public Event(string place, Country country)
    {
        this.Place = place;
        this.Country = country;
    }

    public string Place { get; }
    public Country Country { get; }
    public IReadOnlyCollection<StaffMember> Staff => this.personnelList.AsReadOnly();
    public IReadOnlyCollection<Competition> Competitions => this.competitionList.AsReadOnly();
	
    public void Add(StaffMember staffMember)
    {
        var role = staffMember.Role;
        if (role == StaffRole.PresidentVet ||
            role == StaffRole.PresidentGroundJury ||
            role == StaffRole.ActiveVet ||
            role == StaffRole.FeiDelegateVet ||
            role == StaffRole.FeiDelegateTech ||
            role == StaffRole.ForeignJudge)
        {
            throw new DomainException("Staff  member '", staffMember.Role, "' already exists");
        }
        this.personnelList.Add(staffMember);
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
}
