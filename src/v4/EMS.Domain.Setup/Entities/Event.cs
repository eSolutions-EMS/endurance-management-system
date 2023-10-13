using EMS.Domain.Events.Start;
using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Event : DomainEntity, ISummarizable, IImportable
{
    public Event(string place, Country country)
    {
        this.Place = place;
        this.Country = country;
    }

    public string Place { get; private set; }
    public Country Country { get; private set; }
    public List<Personnel> Personnl { get; private set; } = new();
    public List<Competition> Competitions { get;private set; } = new();
	
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
