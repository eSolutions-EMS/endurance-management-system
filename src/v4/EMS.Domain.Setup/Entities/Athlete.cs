using EMS.Domain.Events.Start;
using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Athlete : DomainEntity, ISummarizable, IImportable
{
    public Athlete(string name, Country country, Club club) //TODO: consider Club as persisted across Events (MAUI's raw resources?)
    {
		this.Person = new Person(name);
		this.Country = country;
		this.Club = club;
	}

	public string? FeiId { get; private set; }
	public Person Person { get; private set; }
	public Country Country { get; private set; }
	public Club Club { get; private set; }

	public override string ToString()
	{
		return $"{this.Person}, {this.Club}, {this.Country}";
	}
}
