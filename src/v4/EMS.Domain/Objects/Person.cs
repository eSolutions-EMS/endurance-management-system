using Common.Domain;

namespace EMS.Domain.Objects;

public record Person : DomainObject
{
    internal static string DELIMITER = " ";

    public Person(string name)
    {
        this.Names = name.Split(DELIMITER);
    }

    public string[] Names { get; private init; } // TODO: test record equality

    public override string ToString()
	{
		return string.Join(DELIMITER, this.Names);
	}

    public static implicit operator string(Person member)
    {
        return member.ToString();
    }
}
