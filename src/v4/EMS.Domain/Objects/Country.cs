using Common.Domain;
using EMS.Domain.Events.Start;

namespace EMS.Domain.Objects;

public record Country(string Name, string IsoCode) : DomainObject, ISummarizable
{
	public string Summarize()
	{
		return $"{this.Name}, {this.IsoCode}";
	}

	public override string ToString()
	{
		return this.Name;
	}
}
