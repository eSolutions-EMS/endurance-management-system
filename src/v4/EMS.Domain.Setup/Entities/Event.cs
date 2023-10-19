using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Event : DomainEntity, ISummarizable, IImportable
{
    private readonly List<Competition> competitionList = new();
    private readonly List<StaffMember> personnelList = new();

    public Event(string place, Country country)
    {
        if (place == "zzz")
        {
            throw new DomainException("ne mi spi tuka");
        }
        this.Place = place;
        this.Country = country;
    }

    public string Place { get; }
    public Country Country { get; }
    public IReadOnlyCollection<StaffMember> Personnl => this.personnelList.AsReadOnly();
    public IReadOnlyCollection<Competition> Competitions => this.competitionList.AsReadOnly();
	
    public string Summarize()
	{
		var summary = new Summarizer(this);
        summary.Add("Personnel".Localize(), this.Personnl);
        summary.Add("Competitions".Localize(), this.Competitions);
        return summary.ToString();
	}
	public override string ToString()
	{
        return $"{this.Place}, {this.Country}";
	}
}
