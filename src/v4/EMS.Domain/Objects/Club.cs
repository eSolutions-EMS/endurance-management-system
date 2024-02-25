using Not.Domain;

namespace EMS.Domain.Objects;

public record Club(string Name) : DomainObject
{
	public override string ToString()
	{
		return this.Name;
	}
}
