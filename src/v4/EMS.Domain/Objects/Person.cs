using Common.Domain;

namespace EMS.Domain.Objects;

public record Person : DomainObject
{
    public Person(string name)
    {
        this.Names = name.Split(" ");
    }

    public string[] Names { get; private init; } // TODO: test record equality

    public override string ToString()
	{
		return string.Join(" ", this.Names);
	}
}
