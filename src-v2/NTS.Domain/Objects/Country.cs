using Not.Domain.Summary;

namespace NTS.Domain.Objects;

public record Country(string IsoCode, string Name) : DomainObject, ISummarizable
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
