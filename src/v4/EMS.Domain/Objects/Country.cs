using Common.Domain;
using Common.Domain.Summary;

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
