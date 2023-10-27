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
